using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.FirstTitle.Client.Resources;

namespace WCA.FirstTitle.Client
{
    public interface IFirstTitleClient
    {
        Task<TResponse> Handle<TResponse>(FirstTitleRequestBase fTRequest, CancellationToken cancellationToken)
            where TResponse: class;

        /// <summary>
        /// Checks credentials against the service. Useful to verify validity of credentials before storing them for later use.
        /// </summary>
        /// <param name="firstTitleCredential">Ther First Title credential to check against the First Title service.</param>
        /// <returns><c>true</c> if the credentials passed the authentication check, otherwise <c>false</c>.</returns>
        /// <exception cref="HttpRequestException">If the check failed due to an infrastructure issue.</exception>
        Task<bool> CheckCredentials(FirstTitleCredential firstTitleCredential);
    }
}