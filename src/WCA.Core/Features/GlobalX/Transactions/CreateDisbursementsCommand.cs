using FluentValidation;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.Resources;
using WCA.Actionstep.Client.Resources.Requests;
using WCA.Actionstep.Client.Resources.Responses;
using WCA.Domain.CQRS;
using WCA.GlobalX.Client.Transactions;

namespace WCA.Core.Features.GlobalX.Transactions
{
    public class CreateDisbursementsCommand : ICommand<TransactionDisbursementRelationship>
    {
        public string ActionstepUserId { get; set; }
        public Transaction Transaction { get; set; }
        public string ActionstepOrgKey { get; set; }
        public int ActionstepMatterId { get; set; }
        public int MinimumMatterIdToSync { get; set; }
        public int TaxCodeIdWithGST { get; set; }
        public int TaxCodeIdNoGST { get; set; }

        public CreateDisbursementsCommand()
        { }

        public class Validator : AbstractValidator<CreateDisbursementsCommand>
        {
            public Validator()
            {
                RuleFor(d => d.ActionstepUserId).NotEmpty();

                RuleFor(d => d.Transaction).NotNull();
                RuleFor(d => d.Transaction.Matter).NotEmpty();
                RuleFor(d => d.Transaction.OrderId).NotEmpty();
                RuleFor(d => d.Transaction.Product).NotNull();
                RuleFor(d => d.Transaction.Product.ProductCode).NotEmpty();
                RuleFor(d => d.Transaction.Product.ProductDescription).NotEmpty();
                RuleFor(d => d.Transaction.SearchReference).NotEmpty();
                RuleFor(d => d.Transaction.User).NotNull();
                RuleFor(d => d.Transaction.User.UserId).NotEmpty();

                RuleFor(d => d.ActionstepOrgKey).NotEmpty();
                RuleFor(d => d.TaxCodeIdWithGST).GreaterThan(0);
                RuleFor(d => d.TaxCodeIdNoGST).GreaterThan(0);
            }
        }

        public class CreateDisbursementsCommandHandler : IRequestHandler<CreateDisbursementsCommand, TransactionDisbursementRelationship>
        {
            private readonly IActionstepService _actionstepService;
            private readonly Validator _validator;

            public CreateDisbursementsCommandHandler(
                IActionstepService actionstepService,
                Validator validator)
            {
                _actionstepService = actionstepService;
                _validator = validator;
            }

            public async Task<TransactionDisbursementRelationship> Handle(CreateDisbursementsCommand request, CancellationToken cancellationToken)
            {
                if (request is null) throw new System.ArgumentNullException(nameof(request));
                _validator.ValidateAndThrow(request);

                var disbursementsToCreate = DisbursementFactory.FromTransaction(
                    request.ActionstepMatterId,
                    request.Transaction,
                    request.TaxCodeIdWithGST,
                    request.TaxCodeIdNoGST);

                var createDisbursementsRequest = new CreateDisbursementsRequest();
                createDisbursementsRequest.Disbursements.AddRange(disbursementsToCreate);
                createDisbursementsRequest.TokenSetQuery = new TokenSetQuery(request.ActionstepUserId, request.ActionstepOrgKey);

                var createdDisbursements = await _actionstepService.Handle<ListDisbursementsResponse>(createDisbursementsRequest);

                var gstTaxableDisbursement = createdDisbursements.Disbursements.SingleOrDefault(d => d.Links.TaxCode == request.TaxCodeIdWithGST);
                var gstFreeDisbursement = createdDisbursements.Disbursements.SingleOrDefault(d => d.Links.TaxCode == request.TaxCodeIdNoGST);

                return new TransactionDisbursementRelationship(
                    request.Transaction.TransactionId,
                    request.ActionstepOrgKey,
                    request.ActionstepMatterId,
                    gstTaxableDisbursement?.Id,
                    gstFreeDisbursement?.Id);
            }
        }
    }
}