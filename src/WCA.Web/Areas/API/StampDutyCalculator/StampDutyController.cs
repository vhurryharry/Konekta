using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WCA.Core.Services;
using WCA.Domain.Models;

namespace WCA.Web.Areas.API.StampDutyCalculator
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class StampDutyController : Controller
    {
        private IStampDutyService stampDutyService;

        public StampDutyController(IStampDutyService stampDutyService)
        {
            this.stampDutyService = stampDutyService;
        }

        [HttpPost]
        [ValidateModelFilter]
        [AllowAnonymous]
        [Produces(typeof(FinancialResults))]
        [ProducesResponseType(typeof(FinancialResults), 200)]
        [ProducesResponseType(typeof(ErrorViewModel), 400)]
        public IActionResult GetStampDutyFeesAndConcessions([FromBody]PropertySaleInformationViewModel propertySaleInformation)
        {
            if (propertySaleInformation is null)
            {
                throw new System.ArgumentNullException(nameof(propertySaleInformation));
            }

            var domainSaleInformation = propertySaleInformation.ToDomainModel();

            try
            {
                return new ObjectResult(stampDutyService.Calculate(domainSaleInformation));
            }
            catch (InvalidPropertySaleInformationException ex)
            {
                return new BadRequestObjectResult(new ErrorViewModel(ex.Message));
            }
        }
    }
}