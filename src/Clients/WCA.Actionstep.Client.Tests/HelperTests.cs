using System;
using System.Threading.Tasks;
using Xunit;

namespace WCA.Actionstep.Client.Tests
{
    public class HelperTests
    {
        [Fact]
        public async void FirstAttemptSucceeds()
        {
            var actionWasCalled = false;

            await Helper.RetryableOperation(
                () => actionWasCalled = true,
                ex => Task.FromResult(false),
                5,
                retryAttempt => TimeSpan.FromMilliseconds(1));

            Assert.True(actionWasCalled);
        }

        [Fact]
        public async void TransientExceptionCausesRetry()
        {
            var actionRunCount = 0;

            await Helper.RetryableOperation(
                () =>
                {
                    actionRunCount++;

                    if (actionRunCount == 1)
                    {
                        throw new ApplicationException("TransientException");
                    }
                },
                ex => Task.FromResult(ex is ApplicationException && ex.Message.Equals("TransientException", StringComparison.InvariantCulture)),
                5,
                retryAttempt => TimeSpan.FromMilliseconds(1));

            // Should succeed on the second run
            Assert.Equal(2, actionRunCount);
        }

        [Fact]
        public async void TooManyRetriesThrows()
        {
            var actionRunCount = 0;

            var ex = await Assert.ThrowsAsync<RetryCountExceededException>(async () =>
            {
                await Helper.RetryableOperation(
                    () =>
                    {
                        actionRunCount++;
                        throw new ApplicationException("AlwaysFails");
                    },
                    ex => Task.FromResult(true),
                    2,
                    retryAttempt => TimeSpan.FromMilliseconds(1));
            });

            // Total run count should be 3. One initial attempt, plut 2 retries.
            Assert.Equal(3, actionRunCount);
            Assert.Equal(3, ex.TotalAttempts);
        }

        [Fact]
        public async void CalculateDelayHasCorrectRetryAttempt()
        {
            var actionRunCount = 0;
            var reportedRetryAttempt = 0;

            await Helper.RetryableOperation(
                () =>
                {
                    actionRunCount++;

                    if (actionRunCount < 3)
                    {
                        throw new ApplicationException();
                    }
                },
                ex => Task.FromResult(ex is ApplicationException),
                5,
                retryAttempt =>
                {
                    // Capture latest retryAttempt for assertion
                    reportedRetryAttempt = retryAttempt;
                    return TimeSpan.FromMilliseconds(1);
                });

            // Should have run three times in total. First two times failing, third time succeeding.
            Assert.Equal(3, actionRunCount);

            // Of the three attempts, two are retries, as the first is the first try.
            Assert.Equal(2, reportedRetryAttempt);
        }
    }
}