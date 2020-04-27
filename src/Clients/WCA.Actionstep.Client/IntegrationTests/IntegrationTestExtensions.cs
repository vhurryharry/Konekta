using System;
using System.Globalization;
using System.Threading.Tasks;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;

namespace WCA.Actionstep.Client.IntegrationTests
{
    public static class IntegrationTestExtensions
    {
        public static async Task CanCreateAndReadDisbursement(this IActionstepService actionstepService, int actionId, TokenSetQuery tokenSetQuery)
        {
            if (actionstepService is null) throw new ArgumentNullException(nameof(actionstepService));
            if (tokenSetQuery is null) throw new ArgumentNullException(nameof(tokenSetQuery));

            var newDisbursement = new Disbursement()
            {
                Description = "Test from API",
                Links =
                {
                    Action = actionId
                }
            };

            var newDisbursementRequest = new CreateDisbursementsRequest()
            {
                Disbursements = { newDisbursement },
                TokenSetQuery = tokenSetQuery
            };

            var result = await actionstepService.Handle<ListDisbursementsResponse>(newDisbursementRequest);
        }
    }
}
