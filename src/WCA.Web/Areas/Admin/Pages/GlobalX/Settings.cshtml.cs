using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WCA.Core.Features.Actionstep;
using WCA.Core.Features.Actionstep.Connection;
using WCA.Core.Features.GlobalX.Settings;
using WCA.Core.Features.GlobalX.Sync;
using WCA.Domain.GlobalX;
using WCA.Web.Pages;

namespace WCA.Web.Areas.Admin.Pages.GlobalX
{
    public class SettingsModel : KonektaPage
    {
        private readonly IMediator _mediator;
        private readonly ILogger<SettingsModel> _logger;

        public SettingsModel(
            IMediator mediator,
            ILogger<SettingsModel> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string EditOrgKey { get; set; }

        [BindProperty]
        public string DeleteOrgKey { get; set; }

        public bool EditOrgKeyMode { get => !string.IsNullOrEmpty(EditOrgKey); }

        [BindProperty]
        public IEnumerable<SelectListItem> AllActionstepOrgs { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> AllUsers { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> AllUsersForOrg { get; set; }

        [BindProperty]
        public IEnumerable<GlobalXOrgSettings> AllSettings { get; set; }

        [BindProperty]
        public GlobalXOrgSettings EditSettings { get; set; }

        public async Task OnGet()
        {
            await PopulateForm();

            if (!string.IsNullOrEmpty(EditOrgKey))
            {
                AllUsersForOrg = (await _mediator.Send(new ActionstepConnectionsForOrgQuery()
                {
                    ActionstepOrgKey = EditOrgKey
                }))
                    .OrderBy(u => u.FirstName)
                    .Select(u => new SelectListItem($"{u.FirstName} {u.LastName} ({u.Email})", u.UserId));

                EditSettings = AllSettings.Single(s => s.ActionstepOrgKey == EditOrgKey);
            }
        }

        public async Task<IActionResult> OnPostDeleteGlobalXSettingsAsync()
        {
            await _mediator.Send(new DeleteGlobalXSettingsCommand()
            {
                ActionstepOrgKey = DeleteOrgKey,
            });

            SuccessMessage = $"Successully deleted settings for org '{DeleteOrgKey}'";

            await PopulateForm();
            return Page();
        }

        public async Task<IActionResult> OnPostEditGlobalXSettingsAsync()
        {
            await _mediator.Send(new SaveGlobalXSettingsCommand()
            {
                ActionstepOrgKey = EditSettings.ActionstepOrgKey,
                GlobalXAdminId = EditSettings.GlobalXAdminId,
                ActionstepSyncUserId = EditSettings.ActionstepSyncUserId,
                TransactionSyncEnabled = EditSettings.TransactionSyncEnabled,
                LatestTransactionId = EditSettings.LatestTransactionId,
                TaxCodeIdWithGST = EditSettings.TaxCodeIdWithGST,
                TaxCodeIdNoGST = EditSettings.TaxCodeIdNoGST,
                DocumentSyncEnabled = EditSettings.DocumentSyncEnabled,
                MinimumMatterIdToSync = EditSettings.MinimumMatterIdToSync,
                LastDocumentSync = Instant.FromDateTimeUtc(DateTime.SpecifyKind(EditSettings.LastDocumentSyncUtc, DateTimeKind.Utc))
            });

            SuccessMessage = $"Successully saved settings for org '{EditSettings.ActionstepOrgKey}'";

            await PopulateForm();
            return Page();
        }

        private async Task PopulateForm()
        {
            EditSettings = new GlobalXOrgSettings();

            // These can run in parallel
            var allActionstepOrgsTask = _mediator.Send(new ActionstepOrgsQuery());
            var allUsersTask = _mediator.Send(new AllUsersQuery());
            var allSettingsTask = _mediator.Send(new GlobalXOrgSettingsQuery());

            Task.WaitAll(allActionstepOrgsTask, allUsersTask, allSettingsTask);

            AllActionstepOrgs = (await allActionstepOrgsTask)
                .Select(o => new SelectListItem(o.Key, o.Key));

            AllUsers = (await allUsersTask)
                .OrderBy(u => u.FirstName)
                .Select(u => new SelectListItem($"{u.FirstName} {u.LastName} ({u.Email})", u.Id));

            AllSettings = await allSettingsTask;
        }
    }
}
