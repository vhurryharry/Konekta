using DinkToPdf;
using DinkToPdf.Contracts;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Core.Features.Actionstep.Queries;
using WCA.Domain.CQRS;
using WCA.Domain.Models.Account;
using WCA.Domain.Models.Settlement;

namespace WCA.Core.Features.Actionstep.Conveyancing.SettlementCalculator
{
    public class SettlementMatterGeneratePDF
    {
        public class SettlementMatterGeneratePDFQuery : IAuthenticatedCommand<string>
        {
            public WCAUser AuthenticatedUser { get; set; }
            public SettlementMatter Matter { get; set; }
        }

        public class Validator : AbstractValidator<SettlementMatterGeneratePDFQuery>
        {
            public Validator()
            {
                RuleFor(c => c.AuthenticatedUser).NotNull();
            }
        }

        public class Handler : IRequestHandler<SettlementMatterGeneratePDFQuery, string>
        {
            private readonly Validator _validator;
            private readonly IMediator _mediator;
            private readonly IConverter _converter;
            private readonly IActionstepService _actionstepService;

            public Handler(
                IConverter converter,
                Validator validator,
                IMediator mediator,
                IActionstepService actionstepService)
            {
                _converter = converter;
                _validator = validator;
                _mediator = mediator;
                _actionstepService = actionstepService;
            }

            public async Task<string> Handle(SettlementMatterGeneratePDFQuery message, CancellationToken token)
            {
                ValidationResult result = _validator.Validate(message);
                if (!result.IsValid)
                {
                    throw new ValidationException("Invalid input.", result.Errors);
                }

                SettlementInfo settlementInfo = message.Matter.SettlementData;

                var matterInfo = await _mediator.Send(new ActionstepMatterInfoQuery(message.Matter.ActionstepOrgKey, message.Matter.ActionstepMatterId, message.AuthenticatedUser));

                var pdfContent = settlementInfo.GeneratePDFContent(matterInfo.OrgName);

                var tempPdfPath = Path.GetTempFileName();

                var doc = new HtmlToPdfDocument()
                {
                    GlobalSettings = {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Out = tempPdfPath
                },
                    Objects = {
                    new ObjectSettings() {
                        PagesCount = true,
                        HtmlContent = pdfContent,
                        WebSettings = {DefaultEncoding = "utf-8"},
                        HeaderSettings = {Line = false, Spacing = 3}
                    }
                }
                };

                _converter.Convert(doc);

                return tempPdfPath;
            }
        }
    }
}
