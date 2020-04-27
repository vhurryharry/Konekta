using AutoMapper;
using DBA.FreshdeskSharp;
using DBA.FreshdeskSharp.Models;
using System;
using System.Threading.Tasks;

namespace WCA.Core.Services.SupportSystem
{
    public class FreshDeskSupportSystem : ISupportSystem
    {
        /// <summary>
        /// See "Type" field for current list: https://workcloud.freshdesk.com/a/admin/ticket_fields
        /// </summary>
        private const string defaultType = "Other";

        /// <summary>
        /// Full list here: https://workcloud.freshdesk.com/a/admin/products
        /// </summary>
        private const long konektaProductId = 6000006798;

        /// <summary>
        /// Full list here: https://workcloud.freshdesk.com/a/admin/groups
        /// </summary>
        private const long konektaGroupId = 6000207669;

        private readonly FreshdeskClient _freshdeskClient;
        private readonly IMapper _mapper;

        public FreshDeskSupportSystem(
            FreshdeskClient freshdeskClient,
            IMapper mapper)
        {
            _freshdeskClient = freshdeskClient;
            _mapper = mapper;
        }

        public async Task<ulong> CreateTicket(NewTicketRequest newTicketRequest)
        {
            if (newTicketRequest is null) throw new ArgumentNullException(nameof(newTicketRequest));

            var newTicket = await _freshdeskClient.Tickets.CreateAsync(new FreshdeskTicket()
            {
                Email = newTicketRequest.FromEmail,
                Subject = newTicketRequest.Subject,
                Priority = _mapper.Map<FreshdeskTicketPriority>(newTicketRequest.TicketPriority),
                Description = newTicketRequest.Description,
                Type = defaultType,
                ProductId = konektaProductId,
                GroupId = konektaGroupId,
                Status = FreshdeskTicketStatus.Open,
                Source = FreshdeskTicketSource.Portal
            });

            return newTicket.Id;
        }
    }
}