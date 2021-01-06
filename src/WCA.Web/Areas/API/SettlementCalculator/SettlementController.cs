using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using System.IO;
using System.Threading.Tasks;
using WCA.Core.Features.Actionstep;
using WCA.Core.Features.Actionstep.Conveyancing.SettlementCalculator;
using WCA.Core.Features.Conveyancing.SettlementCalculator;
using WCA.Domain.Models.Account;
using WCA.Domain.Models.Settlement;

namespace WCA.Web.Areas.API.SettlementCalculator
{

    [Route("api/settlement-calculator")]
    [AllowAnonymous]
    public class SettlementController : Controller
    {
        private readonly UserManager<WCAUser> _userManager;
        private readonly IMediator _mediator;

        public SettlementController(
            UserManager<WCAUser> userManager,
            IMediator mediator
            )
        {
            _userManager = userManager;
            _mediator = mediator;
        }

        [HttpPost]
        [Route("generate-pdf")]
        [ValidateModelFilter]
        [AllowAnonymous]
        [ProducesResponseType(typeof(FileStreamResult), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        public async Task<IActionResult> GeneratePDF([FromBody]SettlementMatterViewModel settlementMatter)
        {
            if (settlementMatter is null)
            {
                throw new System.ArgumentNullException(nameof(settlementMatter));
            }

            var currentUser = await _userManager.GetUserAsync(User);

            var pdfTempFilePath = await _mediator.Send(new SettlementMatterGeneratePDF.SettlementMatterGeneratePDFQuery
            {
                AuthenticatedUser = currentUser,
                Matter = settlementMatter.ToDomainModel()
            });

            var pdfFileStream = new FileStream(pdfTempFilePath, FileMode.Open, FileAccess.Read, FileShare.None, 4096, FileOptions.DeleteOnClose);
            var fileStreamResult = new FileStreamResult(pdfFileStream, new MediaTypeHeaderValue("application/pdf"));
            fileStreamResult.FileDownloadName = $"{settlementMatter.SettlementData.MatterDetails.Matter}.pdf";
            return fileStreamResult;
        }

        [HttpPost]
        [Route("save-pdf")]
        [ValidateModelFilter]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ActionstepDocument), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        public async Task<ActionstepDocument> SavePDF([FromBody]SettlementMatterViewModel settlementMatter)
        {
            if (settlementMatter is null)
            {
                throw new System.ArgumentNullException(nameof(settlementMatter));
            }

            var currentUser = await _userManager.GetUserAsync(User);

            var pdfTempFilePath = string.Empty;

            try
            {
                pdfTempFilePath = await _mediator.Send(new SettlementMatterGeneratePDF.SettlementMatterGeneratePDFQuery
                {
                    AuthenticatedUser = currentUser,
                    Matter = settlementMatter.ToDomainModel()
                });

                return await _mediator.Send(new ActionstepSavePDF.ActionstepSavePDFCommand
                {
                    OrgKey = settlementMatter.ActionstepOrg,
                    MatterId = settlementMatter.MatterId,
                    FilePath = pdfTempFilePath,
                    AuthenticatedUser = currentUser
                });
            }
            finally
            {
                if (!System.IO.File.Exists(pdfTempFilePath))
                {
                    System.IO.File.Delete(pdfTempFilePath);
                }
            }
        }

        [HttpPost]
        [Route("settlement-matter")]
        [ProducesResponseType(typeof(SettlementMatter), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        [AllowAnonymous]
        public async Task<SettlementMatter> SaveSettlementMatter([FromBody]SettlementMatterViewModel settlementMatter)
        {
            if (settlementMatter is null)
            {
                throw new System.ArgumentNullException(nameof(settlementMatter));
            }

            var currentUser = await _userManager.GetUserAsync(User);

            return await _mediator.Send(new SaveSettlementMatter.SaveSettlementMatterCommand
            {
                AuthenticatedUser = currentUser,
                MatterId = settlementMatter.MatterId,
                OrgKey = settlementMatter.ActionstepOrg,
                Matter = settlementMatter.ToDomainModel()
            });
        }

        [HttpDelete]
        [Route("settlement-matter/{actionstepOrg}/{matterId}")]
        [ProducesResponseType(typeof(SettlementMatter), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        [AllowAnonymous]
        public async Task<SettlementMatter> DeleteSettlementMatter(string actionstepOrg, int matterId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            return await _mediator.Send(new DeleteSettlementMatter.DeleteSettlementMatterCommand
            {
                AuthenticatedUser = currentUser,
                MatterId = matterId,
                OrgKey = actionstepOrg
            });
        }

        [HttpGet("settlement-matter/{actionstepOrg}/{matterId}")]
        [Route("settlement-matter")]
        [ProducesResponseType(typeof(SettlementMatterViewModel), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        [AllowAnonymous]
        public async Task<SettlementMatterViewModel> GetSettlementMatter(string actionstepOrg, int matterId)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            return SettlementMatterViewModel.FromDomainModel(await _mediator.Send(new GetSettlementMatter.GetSettlementMatterQuery
            {
                AuthenticatedUser = currentUser,
                MatterId = matterId,
                OrgKey = actionstepOrg
            }));
        }
    }
}