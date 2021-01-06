using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WCA.Core.Services.DurableFunctions;
using WCA.Web.Pages;

namespace WCA.Web.Areas.Admin.Pages
{
    public class OrchestratorInstancesModel : KonektaPage
    {
        private readonly IMediator _mediator;
        private readonly IDurableFunctionsService _durableFunctionsService;
        private readonly ILogger<OrchestratorInstancesModel> _logger;

        public OrchestratorInstancesModel(
            IMediator mediator,
            IDurableFunctionsService durableFunctionsService,
            ILogger<OrchestratorInstancesModel> logger)
        {
            _mediator = mediator;
            _durableFunctionsService = durableFunctionsService;
            _logger = logger;
        }

        [BindProperty]
        public QueryParametersValues QueryParameters { get; set; }

        public class QueryParametersValues
        {
            public DateTime FromDateTime { get; set; }
            public DateTime ToDateTime { get; set; }
            public bool Running { get; set; }
            public bool Pending { get; set; }
            public bool Failed { get; set; }
            public bool Canceled { get; set; }
            public bool Terminated { get; set; }
            public bool Completed { get; set; }
        }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        public IEnumerable<OrchestratorInstance> OrchestratorInstances { get; set; }

        [BindProperty]
        public string InstanceId { get; set; }

        [BindProperty]
        public string[] SelectedInstances { get; set; }

        public async Task<IActionResult> OnGet()
        {
            QueryParameters = new QueryParametersValues()
            {
                FromDateTime = DateTime.UtcNow.Subtract(TimeSpan.FromDays(7)),
                ToDateTime = DateTime.UtcNow,
                Running = true,
                Canceled = true,
                Failed = true,
                Pending = true,
                Terminated = true,
                Completed = false,
            };

            await PopulateForm();
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            await PopulateForm();
            return Page();
        }

        public async Task<IActionResult> OnPostRetryAsync()
        {
            await _durableFunctionsService.RaiseDurableEvent(InstanceId, "RetryFailedActivityEvent");
            SuccessMessage = "Event raised.";
            await PopulateForm();
            return Page();
        }

        public async Task<IActionResult> OnPostRetrySelectedAsync()
        {
            var errors = new List<string>();
            var eventsRaised = 0;
            foreach (var instance in SelectedInstances)
            {
                try
                {
                    await _durableFunctionsService.RaiseDurableEvent(instance, "RetryFailedActivityEvent");
                    eventsRaised++;
                }
                catch(Exception ex)
                {
                    errors.Add(ex.Message);
                }
            }

            SuccessMessage = $"{eventsRaised} events raised.";

            if (errors.Count > 0)
            {
                ErrorMessage = string.Join("<br />", errors);
            }

            await PopulateForm();
            return Page();
        }

        public async Task<IActionResult> OnPostTerminateAsync()
        {
            await _durableFunctionsService.Terminate(InstanceId, "Terminated via Admin UI.");
            SuccessMessage = "Termination requested.";
            await PopulateForm();
            return Page();
        }

        public async Task<IActionResult> OnPostRewindAsync()
        {
            await _durableFunctionsService.Rewind(InstanceId, "Rewind via Admin UI.");
            SuccessMessage = "Rewind requested.";
            await PopulateForm();
            return Page();
        }

        public async Task<IActionResult> OnPostTerminateSelectedAsync()
        {
            var errors = new List<string>();
            var terminationsRequested = 0;
            foreach (var instance in SelectedInstances)
            {
                try
                {
                    await _durableFunctionsService.Terminate(instance, "Terminated via Admin UI.");
                    terminationsRequested++;
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }
            }

            SuccessMessage = $"Sent termination signal for {terminationsRequested} instances.";

            if (errors.Count > 0)
            {
                ErrorMessage = string.Join("<br />", errors);
            }

            await PopulateForm();
            return Page();
        }

        public async Task<IActionResult> OnPostRewindSelectedAsync()
        {
            var errors = new List<string>();
            var rewindsRequested = 0;
            foreach (var instance in SelectedInstances)
            {
                try
                {
                    await _durableFunctionsService.Rewind(instance, "Rewind via Admin UI.");
                    rewindsRequested++;
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }
            }

            SuccessMessage = $"Sent rewind signal for {rewindsRequested} instances.";

            if (errors.Count > 0)
            {
                ErrorMessage = string.Join("<br />", errors);
            }

            await PopulateForm();
            return Page();
        }


        private async Task PopulateForm()
        {
            var statuses = new List<RuntimeStatus>();
            if (QueryParameters.Running) statuses.Add(RuntimeStatus.Running);
            if (QueryParameters.Canceled) statuses.Add(RuntimeStatus.Canceled);
            if (QueryParameters.Failed) statuses.Add(RuntimeStatus.Failed);
            if (QueryParameters.Pending) statuses.Add(RuntimeStatus.Pending);
            if (QueryParameters.Completed) statuses.Add(RuntimeStatus.Completed);

            OrchestratorInstances = await _durableFunctionsService.GetInstances(
                Instant.FromDateTimeUtc(DateTime.SpecifyKind(QueryParameters.FromDateTime, DateTimeKind.Utc)),
                Instant.FromDateTimeUtc(DateTime.SpecifyKind(QueryParameters.ToDateTime, DateTimeKind.Utc)),
                null,
                statuses.ToArray());
        }
    }
}
