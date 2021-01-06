using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.Core.Features.Actionstep.IntegrationTests;
using WCA.Web.Pages;

namespace WCA.Web.Areas.Admin.Pages
{
    public class IntegrationTestsModel : KonektaPage
    {
        private readonly ILogger<IntegrationTestsModel> _logger;

        public IntegrationTestsModel(ILogger<IntegrationTestsModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public ActionstepInputModel ActionstepInput { get; set; }

        public class ActionstepInputModel
        {
            public string OrgKey { get; set; }

            public int MatterId { get; set; }
        }

        [BindProperty]
        public ActionstepResultModel ActionstepResult { get; set; }

        public class ActionstepResultModel
        {
            public bool HasRun { get; set; } = false;
            public bool TestsSuccessful { get; set; }
            public IEnumerable<string> Errors { get; set; }
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostRunActionstepTestsAsync()
        {
            var response = await Mediator.Send(new RunIntegrationTestsCommand(
                ActionstepInput.OrgKey,
                ActionstepInput.MatterId,
                WCAUser));

            ActionstepResult.HasRun = true;
            ActionstepResult.TestsSuccessful = response.TestsSuccessful;
            ActionstepResult.Errors = response.Errors;

            return Page();
        }
    }
}
