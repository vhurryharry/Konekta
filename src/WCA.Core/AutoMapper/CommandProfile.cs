using AutoMapper;
using DBA.FreshdeskSharp.Models;
using NodaTime;
using System;
using System.Text.RegularExpressions;
using WCA.Core.Features;
using WCA.Core.Features.Actionstep.Connection;
using WCA.Core.Features.InfoTrack;
using WCA.Core.Features.ReportSync;
using WCA.Core.Features.SupportSystem;
using WCA.Core.Services.Email;
using WCA.Core.Services.SupportSystem;
using WCA.Domain.Actionstep;
using WCA.Domain.InfoTrack;
using WCA.Domain.Models;
using WCA.PEXA.Client;

namespace WCA.Core.AutoMapper
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            // Required for InfoTrackOrderUpdateMessage
            RecognizeDestinationPrefixes("InfoTrack");

            CreateMap<Instant, DateTime>().ConvertUsing<InstantToDateTimeConverter>();

            CreateMap<SaveIncomingInfoTrackOrderUpdate.SaveIncomingInfoTrackOrderUpdateCommand, InfoTrackOrderUpdateMessage>()
                .ForMember(nameof(InfoTrackOrderUpdateMessage.DateCreatedUtc), m => m.Ignore())
                .ForMember(nameof(InfoTrackOrderUpdateMessage.LastUpdatedUtc), m => m.Ignore())
                .ForMember(nameof(InfoTrackOrderUpdateMessage.CreatedBy), m => m.Ignore())
                .ForMember(nameof(InfoTrackOrderUpdateMessage.UpdatedBy), m => m.Ignore());

            CreateMap<InfoTrackOrderUpdateMessage, InfoTrackOrder>()
                .ForMember(nameof(InfoTrackOrderUpdateMessage.DateCreatedUtc), m => m.Ignore())
                .ForMember(nameof(InfoTrackOrderUpdateMessage.LastUpdatedUtc), m => m.Ignore())
                .ForMember(nameof(InfoTrackOrderUpdateMessage.CreatedBy), m => m.Ignore())
                .ForMember(nameof(InfoTrackOrderUpdateMessage.UpdatedBy), m => m.Ignore());
            
            CreateMap<ActionstepOrg, ConnectedActionstepOrgs.ConnectedActionstepOrgsResponse>();

            CreateMap<ReportSyncSignup.ReportSyncSignupCommand, ReportSyncSignupSubmission>();

            var findWordCaps = new Regex(@"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))");
            CreateMap<InfoTrackOrder, InfoTrackOrderResult>()
                .ForMember(dest => dest.ActionstepDisbursementStatus,
                            m => m.MapFrom(
                                source => findWordCaps.Replace(source.ActionstepDisbursementStatus.ToString(), " $1")))
                .ForMember(dest => dest.ActionstepDocumentUploadStatus,
                            m => m.MapFrom(
                                source => findWordCaps.Replace(source.ActionstepDocumentUploadStatus.ToString(), " $1")))
                .ForMember(dest => dest.ActionstepOrgTitle, m => m.MapFrom(source => source.ActionstepOrg.Title))
                .ForMember(dest => dest.ActionstepOrgKey, m => m.MapFrom(source => source.ActionstepOrg.Key))
                .ForMember(dest => dest.CreatedById, m => m.MapFrom(source => source.CreatedBy.Id))
                .ForMember(dest => dest.CreatedByName, m => m.MapFrom(source => $"{source.CreatedBy.FirstName} {source.CreatedBy.LastName}"))
                .ForMember(dest => dest.CreatedByEmail, m => m.MapFrom(source => source.CreatedBy.Email))
                .ForMember(dest => dest.UpdatedById, m => m.MapFrom(source => source.UpdatedBy.Id))
                .ForMember(dest => dest.UpdatedByName, m => m.MapFrom(source => $"{source.UpdatedBy.FirstName} {source.UpdatedBy.LastName}"))
                .ForMember(dest => dest.UpdatedByEmail, m => m.MapFrom(source => source.UpdatedBy.Email))
                .ForMember(dest => dest.OrderedByWCAUserId, m => m.MapFrom(source => source.OrderedByWCAUser.Id))
                .ForMember(dest => dest.OrderedByWCAUserName, m => m.MapFrom(source => $"{source.OrderedByWCAUser.FirstName} {source.OrderedByWCAUser.LastName}"))
                .ForMember(dest => dest.OrderedByWCAUserEmail, m => m.MapFrom(source => source.OrderedByWCAUser.Email))
                .ForMember(dest => dest.ActionstepDisbursementStatusUpdatedUtc, m => m.MapFrom(source => source.ActionstepDisbursementStatusUpdatedUtc))
                .ForMember(dest => dest.ActionstepDocumentUploadStatusUpdatedUtc, m => m.MapFrom(source => source.ActionstepDocumentUploadStatusUpdatedUtc))
                .ForMember(dest => dest.DateCreatedUtc, m => m.MapFrom(source => source.DateCreatedUtc))
                .ForMember(dest => dest.LastUpdatedUtc, m => m.MapFrom(source => source.LastUpdatedUtc));

            CreateMap<Domain.Conveyancing.ConveyancingMatter, WorkspaceCreationRequest>();
            CreateMap<Domain.Conveyancing.PropertyDetails, WorkspaceCreationRequestTypeLandTitleDetailsLandTitle>()
                .ForMember(dest => dest.LandTitleReference, m => m.MapFrom(source => source.TitleReference))
                .ForMember(dest => dest.UnregisteredLotReference, m => m.MapFrom(source => source.LotNo));

            CreateMap<SendEmailCommand, EmailSenderRequest>();
            CreateMap<TicketPriority, FreshdeskTicketPriority>();
            CreateMap<CreateTicketCommand, NewTicketRequest>()
                .ForMember(dest => dest.Description, m => m.MapFrom(source => source.DescriptionHtml ?? string.Empty));
        }
    }
}
