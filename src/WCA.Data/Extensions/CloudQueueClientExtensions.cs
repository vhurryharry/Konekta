using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Threading.Tasks;

namespace WCA.Data.Extensions
{
    public static class CloudQueueClientExtensions
    {
        public static string InfoTrackResultsQueueName { get; } = "infotrack-results";

        public static async Task<CloudQueue> GetInfoTrackResultsQueue(this CloudQueueClient queueClient)
        {
            if (queueClient is null)
            {
                throw new ArgumentNullException(nameof(queueClient));
            }

            var queueReference = queueClient.GetQueueReference(InfoTrackResultsQueueName);

            try
            {
                await queueReference.CreateIfNotExistsAsync(new QueueRequestOptions()
                {
                    RetryPolicy = new LinearRetry(new TimeSpan(hours: 0, minutes: 10, seconds: 0), maxAttempts: 1)
                }, null);
            }
            catch (Exception ex)
            {
                // We'll wrap the original exception to provide more context for the person investigating.
                throw new ApplicationException("Unable to create InfoTrackResults storage queue.", ex);
            }

            return queueReference;
        }
    }
}
