using System.Collections.Generic;

namespace WCA.Core.Features.Actionstep.IntegrationTests
{
    public class RunIntegrationTestsResponse
    {
        public bool TestsSuccessful { get; }
        public List<string> Errors { get; }

        public RunIntegrationTestsResponse(
            bool testsSuccessful,
            List<string> errors)
        {
            TestsSuccessful = testsSuccessful;
            Errors = errors;
        }
    }
}
