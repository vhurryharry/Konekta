using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WCA.AzureFunctions.GlobalX;
using WCA.Core.Features.GlobalX;
using static WCA.Core.Features.GlobalX.ValidateActionstepMatterCommand;

namespace WCA.AzureFunctions
{
    public static class FunctionsExtensions
    {
        public async static Task<T> CallActivityWithRetryOnEventAsync<T>(
            this IDurableOrchestrationContext context,
            string functionName,
            object input,
            string waitForEventName,
            ILogger logger,
            TimeSpan? delayInterval = null,
            int maxAutoRetries = 4,
            Action<Exception, IDurableOrchestrationContext> onException = null,
            Func<Exception, IDurableOrchestrationContext, bool> shouldAutoRetry = null)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));
            if (string.IsNullOrEmpty(functionName)) throw new ArgumentException("Parameter must be supplied", nameof(functionName));
            if (input is null) throw new ArgumentNullException(nameof(input));
            if (string.IsNullOrEmpty(waitForEventName)) throw new ArgumentException("Parameter must be supplied", nameof(waitForEventName));
            if (logger is null) throw new ArgumentNullException(nameof(logger));

            int retryCount = 0;

            do
            {
                try
                {
                    return await context.CallActivityAsync<T>(functionName, input);
                }
                catch (Exception ex)
                {
                    retryCount++;

                    if (!(onException is null))
                    {
                        try
                        {
                            onException(ex, context);
                        }
                        catch (Exception onExceptionCallbackException)
                        {
                            logger.LogError(onExceptionCallbackException, $"Exception was thrown while running '{nameof(onException)}' callback.");
                        }
                    }

                    using (var timeoutCts = new CancellationTokenSource())
                    {
                        var shouldAutoRetryResult = true;

                        if (!(shouldAutoRetry is null))
                        {
                            try
                            {
                                shouldAutoRetryResult = shouldAutoRetry(ex, context);
                            }
                            catch (Exception shouldAutoRetryCallbackException)
                            {
                                logger.LogError(shouldAutoRetryCallbackException, $"Exception was thrown while running '{nameof(shouldAutoRetry)}' callback.");
                            }
                        }

                        if (retryCount <= maxAutoRetries && shouldAutoRetryResult && delayInterval.HasValue)
                        {
                            DateTime autoRetryTime = context.CurrentUtcDateTime.Add(delayInterval.Value);
                            Task durableTimeout = context.CreateTimer(autoRetryTime, timeoutCts.Token);
                            Task approvalEvent = context.WaitForExternalEvent(waitForEventName);

                            await Task.WhenAny(approvalEvent, durableTimeout);

                            // Cancel the timer in case we have resumed due to an event
                            if (!durableTimeout.IsCompleted)
                            {
                                timeoutCts.Cancel();
                            }
                        }
                        else
                        {
                            // If we've exhausted maxAutoRetries, then just wait for an event before trying again.
                            await context.WaitForExternalEvent(waitForEventName);
                        }
                    }
                }
            }
            while (true);
        }

        public async static Task<int?> ValidateActionstepMatterIdWithRetryAsync(
            this IDurableOrchestrationContext context,
            string matterReference,
            string actionstepOrgKey,
            string actionstepUserId,
            int minimumMatterIdToSync,
            Action<IDurableOrchestrationContext, MatterIdStatus, string> onInvalidResult,
            Action<IDurableOrchestrationContext, string, int> onMatterIdUpdated,
            ILogger logger)
        {
            ActionstepMatterValidationResult actionstepMatterValidationResult = null;
            var matterIdToValidate = matterReference;
            do
            {
                string exceptionMessage = null;

                try
                {
                    var validateActionstepMatterCommand = new ValidateActionstepMatterCommand()
                    {
                        ActionstepOrgKey = actionstepOrgKey,
                        ActionstepUserId = actionstepUserId,
                        MatterId = matterIdToValidate,
                        MinimumMatterIdToSync = minimumMatterIdToSync,
                    };

                    actionstepMatterValidationResult = await context.CallActivityAsync<ActionstepMatterValidationResult>(
                        nameof(SharedActivities.ValidateActionstepMatter),
                        validateActionstepMatterCommand);
                }
                catch (Exception ex)
                {
                    exceptionMessage = ex.Message;
                    logger.LogError(ex, "Exception encountered while validating Actionstep Matter ID.");
                }

                if (actionstepMatterValidationResult?.MatterIdStatus != MatterIdStatus.Valid)
                {
                    var message = GetMatterInvalidReasonMessage(actionstepMatterValidationResult, matterReference, minimumMatterIdToSync, actionstepOrgKey, exceptionMessage);

                    var matterIdStatus = actionstepMatterValidationResult?.MatterIdStatus ?? MatterIdStatus.InvalidUnknownValidationError;

                    if (!(onInvalidResult is null))
                    {
                        try
                        {
                            onInvalidResult(context, matterIdStatus, message);
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, $"Exception encountered while running '{nameof(onInvalidResult)}' callback.");
                        }
                    }

                    // No further processing if the matter doesn't pass the minimum number filter.
                    if (matterIdStatus == MatterIdStatus.InvalidBelowMinimum)
                    {
                        return null;
                    }

                    // Otherwise, wait for an UpdateMatterIdEvent event to fix the matter id.
                    // We expect an int, but convert it back to a string because ValidateActionstepMatterCommand expects a string.
                    var updatedMatterIdEvent = context.WaitForExternalEvent<int>(nameof(Events.UpdateMatterIdEvent));
                    var retryEvent = context.WaitForExternalEvent(nameof(Events.RetryFailedActivityEvent));

                    var winner = await Task.WhenAny(updatedMatterIdEvent, retryEvent);
                    if (winner == updatedMatterIdEvent)
                    {
                        if (!(onMatterIdUpdated is null))
                        {
                            try
                            {
                                onMatterIdUpdated(context, matterIdToValidate, updatedMatterIdEvent.Result);
                            }
                            catch( Exception ex)
                            {
                                logger.LogError(ex, $"Exception encountered while running '{nameof(onMatterIdUpdated)}' callback.");
                            }
                        }

                        matterIdToValidate = updatedMatterIdEvent.Result.ToString(CultureInfo.InvariantCulture);
                    }

                    // If winner is retryEvent, we'll simply loop and retry with the same matter id
                }
            } while (actionstepMatterValidationResult?.MatterIdStatus != MatterIdStatus.Valid);

            return actionstepMatterValidationResult?.MatterId;
        }

        private static string GetMatterInvalidReasonMessage(ActionstepMatterValidationResult actionstepMatterValidationResult, string matterReference, int minimumMatterIdToSync, string actionstepOrgKey, string exceptionMessage)
        {
            var messageBuilder = new StringBuilder();

            if (actionstepMatterValidationResult != null)
            {
                var validatedMatter = actionstepMatterValidationResult?.MatterId?.ToString(CultureInfo.InvariantCulture)
                    ?? matterReference;

                if (actionstepMatterValidationResult.MatterHasBeenMapped)
                {
                    messageBuilder.Append($"Matter has been mapped from '{matterReference}' to '{actionstepMatterValidationResult.MatterId}'. ");
                }

                messageBuilder.Append(actionstepMatterValidationResult.MatterIdStatus switch
                {
                    MatterIdStatus.InvalidUnableToParseAsInt => $"The Matter ID '{matterReference}'" +
                        $" does not appear to be a valid 32-bit integer, so cannot be a valid Actionstep Matter ID.",
                    MatterIdStatus.InvalidBelowMinimum => $"Matter ID '{validatedMatter}'" +
                        $" is below the minimum configured number of '{minimumMatterIdToSync}' so will be skipped.",
                    MatterIdStatus.InvalidNotFoundInActionstep => $"The Matter ID '{validatedMatter}'" +
                        $" was not found in the Actionstep org with key '{actionstepOrgKey}'.",
                    _ => $"The Matter ID '{validatedMatter}' could not be validated but we're not sure why.",
                });
            }

            if (!string.IsNullOrEmpty(exceptionMessage))
            {
                messageBuilder.Append($" The following error was encountered: {exceptionMessage}");
            }

            return messageBuilder.ToString();
        }
    }
}
