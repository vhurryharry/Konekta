using FluentValidation;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.FirstTitle.Client;

namespace WCA.Core.Features.FirstTitle.Connection
{
    public class CheckFirstTitleCredentialsQueryHandler : IRequestHandler<CheckFirstTitleCredentialsQuery, bool>
    {
        private readonly CheckFirstTitleCredentialsQuery.Validator _validator;
        private readonly IFirstTitleClient _firstTitleClient;

        public CheckFirstTitleCredentialsQueryHandler(
            IFirstTitleClient firstTitleClient,
            CheckFirstTitleCredentialsQuery.Validator validator)
        {
            _firstTitleClient = firstTitleClient;
            _validator = validator;
        }

        public async Task<bool> Handle(CheckFirstTitleCredentialsQuery query, CancellationToken cancellationToken)
        {
            try
            {
                await _validator.ValidateAndThrowAsync(query);

                return await _firstTitleClient.CheckCredentials(query.FirstTitleCredentials);
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
