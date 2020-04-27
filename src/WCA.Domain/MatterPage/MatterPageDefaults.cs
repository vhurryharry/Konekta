//using System;
//using System.Diagnostics.CodeAnalysis;
//using WCA.Domain.Actionstep;
//using WCA.Domain.Integrations;
//using WCA.Domain.Models.Account;

//namespace WCA.Domain.MatterPage
//{
//    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Seed data")]
//    public static class MatterPageDefaults
//    {
//        public static MatterPageSettings[] MatterPageSettings
//        {
//            get
//            {
//                var createdDate = DateTime.UtcNow;
//                return new[]
//                {
//                    new MatterPageSettings()
//                    {
//                        ActionstepOrgKey = ActionstepDefaults.AllOrgsKey,
//                        UserId = WCAUser.AllUsersId,
//                        DateCreatedUtc = createdDate,
//                        LastUpdatedUtc = createdDate,
//                        EnabledIntegrations =
//                        {
//                            IntegrationsDefaults.PexaIntegrationId,
//                            IntegrationsDefaults.InfoTrackIntegrationId,
//                            IntegrationsDefaults.ConveyancingCalculatorsIntegrationId,
//                            IntegrationsDefaults.FirstTitleIntegrationId,
//                            IntegrationsDefaults.GlobalXIntegrationId,
//                            IntegrationsDefaults.TheSearchPeopleIntegrationId,
//                        },
//                        EnabledIntegrationsLinks =
//                        {
//                            IntegrationsDefaults.CreateWorkspaceIntegrationLinkId,
//                            IntegrationsDefaults.PropertyInquiryIntegrationLinkId,
//                            IntegrationsDefaults.TitleSearchIntegrationLinkId,
//                            IntegrationsDefaults.OrderHistoryIntegrationLinkId,
//                            IntegrationsDefaults.SettlementCalculatorIntegrationLinkId,
//                            IntegrationsDefaults.StampDutyCalculatorIntegrationLinkId,
//                            IntegrationsDefaults.OrderPolicyIntegrationLinkId,
//                            IntegrationsDefaults.PropertyInformationIntegrationLinkId,
//                            IntegrationsDefaults.ComingSoonIntegrationLinkId,
//                        },
//                    }
//                };
//            }
//        }
//    }
//}