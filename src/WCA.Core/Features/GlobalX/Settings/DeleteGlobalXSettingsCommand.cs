using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.CQRS;

namespace WCA.Core.Features.GlobalX.Sync
{
    public class DeleteGlobalXSettingsCommand : ICommand
    {
        public string ActionstepOrgKey { get; set; }

        public class Validator : AbstractValidator<DeleteGlobalXSettingsCommand>
        {
            public Validator()
            {
                RuleFor(c => c.ActionstepOrgKey).NotEmpty();
            }
        }

        public class DeleteGlobalXSettingsCommandHandler : IRequestHandler<DeleteGlobalXSettingsCommand>
        {
            private readonly Validator _validator;
            private readonly WCADbContext _wCADbContext;

            public DeleteGlobalXSettingsCommandHandler(
                WCADbContext wCADbContext,
                Validator validator)
            {
                _validator = validator;
                _wCADbContext = wCADbContext;
            }

            public async Task<Unit> Handle(DeleteGlobalXSettingsCommand request, CancellationToken cancellationToken)
            {
                if (request is null) throw new ArgumentNullException(nameof(request));
                _validator.ValidateAndThrow(request);

                var settings = await _wCADbContext.GlobalXOrgSettings
                    .SingleOrDefaultAsync(g => g.ActionstepOrgKey == request.ActionstepOrgKey);

                if (!(settings is null))
                {
                    _wCADbContext.GlobalXOrgSettings.Remove(settings);
                    await _wCADbContext.SaveChangesAsync();
                }

                return new Unit();
            }
        }
    }
}
