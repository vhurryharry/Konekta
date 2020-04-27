using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Options;
using NodaTime;
using System;
using System.Threading;
using System.Threading.Tasks;
using WCA.Core.Services.Email;
using WCA.Core.Services.SupportSystem;
using WCA.Domain.CQRS;

namespace WCA.Core.Features.SupportSystem
{
    public class CreateTicketCommand : ICommand<CreateTicketCommand.CreateTicketResponse>
    {
        public string FromEmail { get; set; }
        public string Subject { get; set; }

        public TicketPriority TicketPriority { get; set; }

        /// <summary>
        /// HTML is allowed.
        /// </summary>
        public string DescriptionHtml { get; set; }

        public class Validator : AbstractValidator<CreateTicketCommand>
        {
            public Validator()
            {
                RuleFor(c => c.FromEmail).NotEmpty();
                RuleFor(c => c.Subject).NotEmpty();
                RuleFor(c => c.DescriptionHtml).NotEmpty();
            }
        }

        public class CreateTicketResponse
        {
            public CreateTicketResponse(ulong id)
            {
                Id = id;
            }

            public ulong Id { get; }
        }

        public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, CreateTicketResponse>
        {
            private readonly ISupportSystem _supportSystem;
            private readonly IEmailSender _emailSender;
            private readonly IClock _clock;
            private readonly IMapper _mapper;

            private readonly bool _redirectToEmail;
            private readonly string _redirectToEmailRecipient;

            public CreateTicketCommandHandler(
                ISupportSystem supportSystem,
                IEmailSender emailSender,
                IClock clock,
                IOptions<WCACoreSettings> options,
                IMapper mapper)
            {
                if (options is null) throw new ArgumentNullException(nameof(options));

                _supportSystem = supportSystem ?? throw new ArgumentNullException(nameof(supportSystem));
                _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
                _clock = clock;
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

                _redirectToEmailRecipient = options.Value.RedirectTicketsToEmail;
                _redirectToEmail = !string.IsNullOrEmpty(_redirectToEmailRecipient);
            }

            public async Task<CreateTicketResponse> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
            {
                if (request is null) throw new ArgumentNullException(nameof(request));

                if (_redirectToEmail)
                {
                    await _emailSender.SendEmailAsync(new EmailSenderRequest()
                    {
                        To = { new EmailRecipient(_redirectToEmailRecipient) },
                        Subject = $"Redirected Ticket: {request.Subject}",
                        MessageIsHtml = true,
                        Message = request.DescriptionHtml
                    });

                    return new CreateTicketResponse((ulong)_clock.GetCurrentInstant().ToUnixTimeMilliseconds());
                }
                else
                {
                    return new CreateTicketResponse(await _supportSystem.CreateTicket(_mapper.Map<NewTicketRequest>(request)));
                }
            }
        }
    }
}
