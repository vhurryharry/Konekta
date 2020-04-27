using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Data;
using WCA.Domain.GlobalX;

namespace WCA.Core.Features.GlobalX.Documents
{
    public class DocumentVersionMatterIdUpdatedMatterMappingHandler : INotificationHandler<DocumentVersionMatterIdUpdated>
    {
        private readonly WCADbContext _wCADbContext;

        public DocumentVersionMatterIdUpdatedMatterMappingHandler(
            WCADbContext wCADbContext)
        {
            _wCADbContext = wCADbContext;
        }

        public async Task Handle(DocumentVersionMatterIdUpdated notification, CancellationToken cancellationToken)
        {
            if (notification is null) throw new ArgumentNullException(nameof(notification));

            var mapping = await _wCADbContext.GlobalXMatterMappings.FindAsync(notification.ActionstepOrgKey, notification.OldMatterId);

            if (mapping is null)
            {
                mapping = new GlobalXMatterMapping();
                mapping.ActionstepOrgKey = notification.ActionstepOrgKey;
                mapping.GlobalXMatterId = notification.OldMatterId;
                _wCADbContext.GlobalXMatterMappings.Add(mapping);
            }

            mapping.ActionstepMatterId = notification.NewMatterId;

            await _wCADbContext.SaveChangesAsync();
        }
    }
}
