using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WCA.Core.Extensions
{
    public static class MediatorExtensions
    {
        /// <summary>
        /// Publishes notifications within a try catch block. If any errors are encountered, they will be logged
        /// to the supplied <see cref="ILogger"/>. Will only throw a <see cref="ArgumentNullException"/>
        /// if <paramref name="mediator"/> is null.
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="notification"></param>
        /// <param name="logger"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task PublishAndLogExceptions(this IMediator mediator, Object notification, ILogger logger, CancellationToken cancellationToken = default)
        {
            if (mediator is null) throw new ArgumentNullException(nameof(mediator));

            try
            {
                await mediator.Publish(notification, cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "There was an error with one or more notification handlers.");
            }
        }
    }
}
