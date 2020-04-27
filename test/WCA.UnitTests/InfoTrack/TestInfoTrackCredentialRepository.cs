using System.Threading.Tasks;
using WCA.Core.Features.InfoTrack;
using WCA.Domain.InfoTrack;

namespace WCA.UnitTests.InfoTrack
{
    internal class TestInfoTrackCredentialRepository : IInfoTrackCredentialRepository
    {
        private InfoTrackCredentials singleSetOfCredentials;

        /// <summary>
        /// Stores a single set of credentials. Will return the credential if the org matches.
        /// 
        /// If SaveOrUpdateCredential is called, the single set of internal credentials will
        /// be overwritten
        /// </summary>
        /// <param name="orgToReturn"></param>
        /// <param name="usernameToReturn"></param>
        /// <param name="passwordToReturn"></param>
        public TestInfoTrackCredentialRepository(string actionstepOrgKey, string username, string password)
        {
            SaveOrUpdateCredential(actionstepOrgKey, username, password).Wait();
        }

        public TestInfoTrackCredentialRepository()
        {
        }

        public Task<InfoTrackCredentials> FindCredential(string actionstepOrgKey)
        {
            return Task.FromResult(singleSetOfCredentials);
        }

        public Task SaveOrUpdateCredential(string actionstepOrgKey, string username, string password)
        {
            singleSetOfCredentials = new InfoTrackCredentials()
            {
                ActionstepOrgKey = actionstepOrgKey,
                Username = username,
                Password = password
            };

            return Task.CompletedTask;
        }

        /// <summary>
        /// Removes the internally cached credentials, setting them to null.
        /// </summary>
        public void Reset()
        {
            singleSetOfCredentials = null;
        }
    }
}