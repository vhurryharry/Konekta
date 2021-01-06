using AutoMapper;
using WCA.Core.Features.ReportSync;
using WCA.Web.Areas.API.Customer;

namespace WCA.Web.AutoMapper
{
    // Configuration specific to the WCA.Web project.
    // Please use the WCA.Core AutoMapper configuration to configure
    // mappings that don't depend on anything in the web project.
    public class WebAutoMapperProfile : Profile
    {
        public WebAutoMapperProfile()
        {
            CreateMap<NewCustomerViewModel, ReportSyncSignup.ReportSyncSignupCommand>();
            CreateMap<Core.Features.Actionstep.Responses.ActionstepMatterInfo, Areas.API.Actionstep.ActionstepMatterInfo>();
        }
    }
}
