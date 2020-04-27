using System;
using System.Threading.Tasks;

namespace WCA.Actionstep.Client
{
    public static class Helper
    {
        /// <summary>
        /// For use with ArgumentException to provide a consistent message for strings that are null or empty (as opposed to using ArgumentNullException).
        /// </summary>
        /// <param name="paramName"></param>
        internal const string NullOrEmptyParameterString = "The parameter was null or empty.";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TransientOperationAsync">The operation to perform.</param>
        /// <param name="IsTransient">A function to decide whether an exception is transient or not. If transient, this function should return <see cref="true"/> which will enable the next retry.</param>
        /// <param name="retryCount">How many times the operation should be retried.</param>
        /// <param name="delay">The delay between retries.</param>
        /// <returns></returns>
        internal async static Task RetryableOperation(
            Action transientOperationAsync,
            Func<Exception, Task<bool>> isTransient,
            int retryCount,
            Func<int, TimeSpan> calculateNextDelay)
        {
            var success = await RetryableOperation(
                () => { transientOperationAsync(); return Task.FromResult(true); },
                isTransient,
                retryCount,
                calculateNextDelay);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="TransientOperationAsync">The operation to perform.</param>
        /// <param name="IsTransient">A function to decide whether an exception is transient or not. If transient, this function should return <see cref="true"/> which will enable the next retry.</param>
        /// <param name="retryCount">How many times the operation should be retried.</param>
        /// <param name="calculateNextDelay">Calculate the delay for the next retry. Includes the retryAttempt to allow for an exponential back-off.
        ///     For example :
        ///     <c>retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))</c>
        ///
        ///     Or you can just return a fixed TimeSpan:
        ///     <c>retryAttempt => TimeSpan.FromSeconds(2)</c>
        /// </param>
        /// <returns></returns>
        internal async static Task<T> RetryableOperation<T>(
            Func<Task<T>> transientOperationAsync,
            Func<Exception, Task<bool>> isTransient,
            int retryCount,
            Func<int, TimeSpan> calculateNextDelay)
        {
            int retryAttempt = 0;

            while (true)
            {
                try
                {
                    // Call external service.
                    return await transientOperationAsync.Invoke().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    retryAttempt++;

                    // Check if the exception thrown was a transient exception
                    // based on the logic in the error detection strategy.
                    // Determine whether to retry the operation, as well as how
                    // long to wait, based on the retry strategy.
                    if (!await isTransient(ex))
                    {
                        // If this isn't a transient error or we shouldn't retry,
                        // rethrow the exception.
                        throw;
                    }
                }

                if (retryAttempt > retryCount)
                {
                    // retryAttempt will have already incremented, so will actually reflect the total number of
                    // attempts which includes the first attempt (as the first attempt is not a retry).
                    throw new RetryCountExceededException(retryAttempt);
                }

                // Wait to retry the operation.
                // Consider calculating an exponential delay here and
                // using a strategy best suited for the operation and fault.
                await Task.Delay(calculateNextDelay(retryAttempt));
            }
        }
    }
}