using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WCA.Actionstep.Client;
using WCA.Actionstep.Client.IntegrationTests;
using WCA.Actionstep.Client.Resources;

namespace WCA.Core.Features.Actionstep.IntegrationTests
{
    public class RunIntegrationTestsHandler : IRequestHandler<RunIntegrationTestsCommand, RunIntegrationTestsResponse>
    {
        private readonly RunIntegrationTestsCommand.Validator _validator;
        private readonly IActionstepService _actionstepService;

        public RunIntegrationTestsHandler(
            RunIntegrationTestsCommand.Validator validator,
            IActionstepService actionstepService)
        {
            _validator = validator;
            _actionstepService = actionstepService;
        }

        public async Task<RunIntegrationTestsResponse> Handle(RunIntegrationTestsCommand message, CancellationToken token)
        {
            if (message is null) throw new ArgumentNullException(nameof(message));

            ValidationResult result = _validator.Validate(message);
            if (!result.IsValid)
            {
                throw new ValidationException("Invalid input.", result.Errors);
            }

            bool testsSuccessful = true;
            List<string> errors = new List<string>();

            try
            {
                await _actionstepService.CanCreateAndReadDisbursement(message.MatterId, new TokenSetQuery(message.AuthenticatedUser.Id, message.OrgKey));
            }
            catch (Exception ex)
            {
                testsSuccessful = false;
                errors.Add(ex.Message);
            }

            return new RunIntegrationTestsResponse(testsSuccessful, errors);
        }
    }
}