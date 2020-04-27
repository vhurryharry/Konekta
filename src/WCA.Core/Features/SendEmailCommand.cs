using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Services.Email;
using WCA.Domain.CQRS;

namespace WCA.Core.Features
{
    public class SendEmailCommand : ICommand
    {
        public List<EmailRecipient> To { get; } = new List<EmailRecipient>();
        public List<EmailRecipient> Cc { get; } = new List<EmailRecipient>();
        public string Subject { get; set; }
        public string Message { get; set; }
        public string TemplateId { get; set; }
        public bool MessageIsHtml { get; set; } = true;
        public Dictionary<string, string> Substitutions { get; } = new Dictionary<string, string>();

        public class Handler : AsyncRequestHandler<SendEmailCommand>
        {
            private readonly IEmailSender _emailSender;
            private readonly IMapper _mapper;

            public Handler(IEmailSender emailSender, IMapper mapper)
            {
                _emailSender = emailSender;
                _mapper = mapper;
            }

            protected override async Task Handle(SendEmailCommand request, CancellationToken cancellationToken)
            {
                if (request is null) throw new ArgumentNullException(nameof(request));

                await _emailSender.SendEmailAsync(_mapper.Map<EmailSenderRequest>(request));
            }
        }
    }
}
