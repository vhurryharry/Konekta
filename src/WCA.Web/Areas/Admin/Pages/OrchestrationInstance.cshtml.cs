using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.Core.Services.DurableFunctions;
using WCA.Web.Pages;

namespace WCA.Web.Areas.Admin.Pages
{
    public class OrchestrationInstanceModel : KonektaPage
    {
        private readonly IDurableFunctionsService _durableFunctionsService;

        public OrchestrationInstanceModel(
            IDurableFunctionsService durableFunctionsService)
        {
            _durableFunctionsService = durableFunctionsService;
        }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        [BindProperty]
        public int NewMatterId { get; set; }

        public OrchestratorInstance OrchestratorInstance { get; set; }

        public IEnumerable<OrchestratorInstance> OrchestratorInstances { get; set; }

        public async Task OnGet(string id)
        {
            await PopulateForm(id);
        }

        private async Task PopulateForm(string id)
        {
            OrchestratorInstance = await _durableFunctionsService.GetInstance(id);
        }

        public async Task OnPostRaiseRetryEventAsync(string instanceId)
        {
            await _durableFunctionsService.RaiseDurableEvent(instanceId, "RetryFailedActivityEvent");
            SuccessMessage = "Event raised";

            await PopulateForm(instanceId);
        }

        public async Task OnPostTerminateAsync(string instanceId)
        {
            await _durableFunctionsService.Terminate(instanceId, "Terminated via Admin UI.");
            SuccessMessage = "Termination requested";

            await PopulateForm(instanceId);
        }

        public async Task OnPostRewindAsync(string instanceId)
        {
            await _durableFunctionsService.Rewind(instanceId, "Rewind via Admin UI.");
            SuccessMessage = "Rewind requested";

            await PopulateForm(instanceId);
        }

        public async Task OnPostRaiseUpdateMatterIdEventAsync(string instanceId)
        {
            if (NewMatterId > 0)
            {
                await _durableFunctionsService.RaiseDurableEvent(instanceId, "UpdateMatterIdEvent", NewMatterId);
                SuccessMessage = "Event raised";
            }
            else
            {
                ErrorMessage = "New Matter Id must be greater than zero.";
            }

            await PopulateForm(instanceId);
        }
    }
}
