using System;
using System.Threading.Tasks;

namespace WCA.Core.Helpers
{
    public static class RetryHelper
    {
        public static async Task RetryOnExceptionAsync(
            int times, Func<Task> operation)
        {
            await RetryOnExceptionAsync<Exception>(times, operation);
        }

        public static async Task RetryOnExceptionAsync<TException>(
            int times, Func<Task> operation) where TException : Exception
        {
            if (times <= 0)
                throw new ArgumentOutOfRangeException(nameof(times));

            var attempts = 0;
            do
            {
                try
                {
                    attempts++;
                    await operation();
                    break;
                }
                catch (TException ex)
                {
                    if (attempts == times)
                        throw;

                    await CreateDelayForException(times, attempts, ex);
                }
            } while (true);
        }

        private static Task CreateDelayForException(
            int times, int attempts, Exception ex)
        {
            int delay = (int)TimeSpan.FromSeconds(20).TotalMilliseconds;
            return Task.Delay(delay);
        }
    }
}