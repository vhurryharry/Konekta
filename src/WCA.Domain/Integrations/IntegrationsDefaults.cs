using System;
using System.Diagnostics.CodeAnalysis;
using WCA.Domain.Actionstep;
using WCA.Domain.Models.Account;

namespace WCA.Domain.Integrations
{
    /// <summary>
    /// Uses Guid identifier to allow for seeding and consistent base set of integrations.
    /// </summary>
    [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Seed data")]
    public static class IntegrationsDefaults
    {
        #region Integration IDs
        public static Guid PexaIntegrationId                            { get => new Guid("DDE37C19-C431-4406-A0C5-2670085B84B0"); }
        public static Guid InfoTrackIntegrationId                       { get => new Guid("5F9C8E97-DEAB-42F9-8678-0FB55DA7D53C"); }
        public static Guid ConveyancingCalculatorsIntegrationId         { get => new Guid("D89CC4AC-E709-41A1-AA08-DD47C728B88F"); }
        public static Guid FirstTitleIntegrationId                      { get => new Guid("4B5C4F21-AD76-4847-96A5-C067865FFF5B"); }
        public static Guid GlobalXIntegrationId                         { get => new Guid("40760360-A77C-4A5A-AF9E-B03321909E80"); }
        public static Guid TheSearchPeopleIntegrationId                 { get => new Guid("F411B5E1-2762-4374-AB81-228B7B13B22A"); }
        #endregion Integration IDs

        #region Integration Link IDs
        public static Guid CreateWorkspaceIntegrationLinkId             { get=> new Guid("E2648BF7-F4D4-48C3-8CF7-16338196D991"); }
        public static Guid PropertyInquiryIntegrationLinkId             { get=> new Guid("BEF737BC-7E50-4D6F-B662-8AE2FC511B76"); }
        public static Guid TitleSearchIntegrationLinkId                 { get=> new Guid("DA695C2E-0FD6-4BEA-A845-4F27ECF686C5"); }
        public static Guid OrderHistoryIntegrationLinkId                { get=> new Guid("D472CF72-6346-4CFC-9E3B-3E2EFB5F19D2"); }
        public static Guid SettlementCalculatorIntegrationLinkId        { get=> new Guid("ABB6D9DB-516D-4E14-BEC7-A34CE8E1DCFB"); }
        public static Guid StampDutyCalculatorIntegrationLinkId         { get=> new Guid("9DE6F462-CD7D-4776-B253-FF5C170E9120"); }
        public static Guid OrderPolicyIntegrationLinkId                 { get=> new Guid("C4239316-39A5-4B44-9918-A73A223FE6DF"); }
        public static Guid GlobalXPropertyInformationIntegrationLinkId  { get=> new Guid("03A39A32-5FD3-4F09-A616-1FAB239725CA"); }
        public static Guid GlobalXAllProductsLinkId                     { get=> new Guid("4B418018-0FB8-47B3-AC0D-0384E983FFC3"); }
        public static Guid TheSearchPeopleComingSoonIntegrationLinkId   { get=> new Guid("4FADB8B3-0655-4B8A-8E62-F15A623E28A5"); }
        #endregion Integration Link IDs

        public static Integration[] Integrations
        {
            get
            {
                return new[]
                {
                    new Integration() { Id = PexaIntegrationId                     , Title = "PEXA"                      , ComingSoon = false  , LogoHref = "https://www.pexa.com.au/"         , LogoAlt = "PEXA Logo"               , LogoSrc = "/images/pexa-logo.svg"                 , LogoWidth = "100px"  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new Integration() { Id = InfoTrackIntegrationId                , Title = "InfoTrack"                 , ComingSoon = false  , LogoHref = "https://www.infotrack.com.au/"    , LogoAlt = "InfoTrack Logo"          , LogoSrc = "/images/InfoTrackLogo_216x80.png"      , LogoWidth = "100px"  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new Integration() { Id = ConveyancingCalculatorsIntegrationId  , Title = "Conveyancing Calculators"  , ComingSoon = false  , LogoHref = "https://www.konekta.com.au/"      , LogoAlt = "Calculators icon"        , LogoSrc = "/images/conveyancing-calculators.png"  , LogoWidth = "80px"   , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new Integration() { Id = FirstTitleIntegrationId               , Title = "First Title"               , ComingSoon = true   , LogoHref = "https://www.firsttitle.com.au/"   , LogoAlt = "First Title Logo"        , LogoSrc = "/images/firsttitle-logo.svg"           , LogoWidth = "100px"  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new Integration() { Id = GlobalXIntegrationId                  , Title = "GlobalX"                   , ComingSoon = true   , LogoHref = "https://globalx.com.au/"          , LogoAlt = "GlobalX Logo"            , LogoSrc = "/images/globalx-logo.png"              , LogoWidth = "150px"  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new Integration() { Id = TheSearchPeopleIntegrationId          , Title = "The Search People"         , ComingSoon = true   , LogoHref = "https://thesearchpeople.com.au/"  , LogoAlt = "The Search People Logo"  , LogoSrc = "/images/the-search-people-logo.png"    , LogoWidth = "150px"  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                };
            }
        }

        public static IntegrationSetting[] IntegrationSettings
        {
            get
            {
                return new[]
                {
                    new IntegrationSetting() { IntegrationId = PexaIntegrationId                     , SortOrder = 10  , ActionstepOrgKey = ActionstepDefaults.AllOrgsKey  , UserId = WCAUser.AllUsersId  , Id = new Guid("09178645-669F-4D5D-9055-52166A6C9C23")  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationSetting() { IntegrationId = InfoTrackIntegrationId                , SortOrder = 20  , ActionstepOrgKey = ActionstepDefaults.AllOrgsKey  , UserId = WCAUser.AllUsersId  , Id = new Guid("0785F5B2-1A61-478B-A049-12F1C78ED155")  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationSetting() { IntegrationId = ConveyancingCalculatorsIntegrationId  , SortOrder = 30  , ActionstepOrgKey = ActionstepDefaults.AllOrgsKey  , UserId = WCAUser.AllUsersId  , Id = new Guid("CB2C8DAC-3177-4B23-894E-0833B5840B86")  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationSetting() { IntegrationId = FirstTitleIntegrationId               , SortOrder = 40  , ActionstepOrgKey = ActionstepDefaults.AllOrgsKey  , UserId = WCAUser.AllUsersId  , Id = new Guid("F6A6DAA1-096A-41F3-8357-82843DA3555A")  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationSetting() { IntegrationId = GlobalXIntegrationId                  , SortOrder = 50  , ActionstepOrgKey = ActionstepDefaults.AllOrgsKey  , UserId = WCAUser.AllUsersId  , Id = new Guid("10DB2716-631E-4445-A6FF-37C8F7E02D26")  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationSetting() { IntegrationId = TheSearchPeopleIntegrationId          , SortOrder = 60  , ActionstepOrgKey = ActionstepDefaults.AllOrgsKey  , UserId = WCAUser.AllUsersId  , Id = new Guid("8C3DA309-CD33-4655-85FF-097FC4F1DFD2")  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                };
            }
        }

        public static IntegrationLink[] IntegrationLinks
        {
            get
            {
                return new[]
                {
                    new IntegrationLink() { IntegrationId = PexaIntegrationId                     , Id = CreateWorkspaceIntegrationLinkId              , Title = "Create Workspace"       , Href = "/pexa/create-workspace?actionsteporg={actionstepOrg}&matterid={matterId}"                                                         , OpenInNewWindow = false  , IsReactLink = true   , IsBeta = false  , Disabled = false  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLink() { IntegrationId = InfoTrackIntegrationId                , Id = PropertyInquiryIntegrationLinkId              , Title = "Property Inquiry"       , Href = "/wca/infotrack/redirect-with-matter-info?resolvableEntryPoint=PropertyEnquiry&matterId={matterId}&actionstepOrg={actionstepOrg}"  , OpenInNewWindow = true   , IsReactLink = false  , IsBeta = false  , Disabled = false  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLink() { IntegrationId = InfoTrackIntegrationId                , Id = TitleSearchIntegrationLinkId                  , Title = "Title Search"           , Href = "/wca/infotrack/redirect-with-matter-info?resolvableEntryPoint=TitleSearch&matterId={matterId}&actionstepOrg={actionstepOrg}"      , OpenInNewWindow = true   , IsReactLink = false  , IsBeta = false  , Disabled = false  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLink() { IntegrationId = InfoTrackIntegrationId                , Id = OrderHistoryIntegrationLinkId                 , Title = "Order History"          , Href = "/wca/infotrack/orders"                                                                                                            , OpenInNewWindow = false  , IsReactLink = false  , IsBeta = false  , Disabled = false  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLink() { IntegrationId = ConveyancingCalculatorsIntegrationId  , Id = SettlementCalculatorIntegrationLinkId         , Title = "Settlement Calculator"  , Href = "/api/conveyancing/old-settlement-calculator/redirect-with-matter-data/{actionstepOrg}/{matterId}"                                 , OpenInNewWindow = true   , IsReactLink = false  , IsBeta = false  , Disabled = false  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLink() { IntegrationId = ConveyancingCalculatorsIntegrationId  , Id = StampDutyCalculatorIntegrationLinkId          , Title = "Stamp Duty Calculator"  , Href = "/wca/stamp-duty-calculator"                                                                                                       , OpenInNewWindow = false  , IsReactLink = false  , IsBeta = false  , Disabled = false  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLink() { IntegrationId = FirstTitleIntegrationId               , Id = OrderPolicyIntegrationLinkId                  , Title = "Order Policy"           , Href = "/firsttitle/request-policy?actionsteporg={actionstepOrg}&matterid={matterId}"                                                     , OpenInNewWindow = false  , IsReactLink = true   , IsBeta = true   , Disabled = false  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLink() { IntegrationId = GlobalXIntegrationId                  , Id = GlobalXPropertyInformationIntegrationLinkId   , Title = "Property Information"   , Href = "/globalx/property-information?actionsteporg={actionstepOrg}&matterid={matterId}&entryPoint=propertyinformation&embed=true"        , OpenInNewWindow = true   , IsReactLink = true   , IsBeta = true   , Disabled = false  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLink() { IntegrationId = GlobalXIntegrationId                  , Id = GlobalXAllProductsLinkId                      , Title = "All Products"           , Href = "/globalx/property-information?actionsteporg={actionstepOrg}&matterid={matterId}"                                                  , OpenInNewWindow = true   , IsReactLink = true   , IsBeta = true   , Disabled = false  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                };
            }
        }

        public static IntegrationLinkSetting[] IntegrationLinkSettings
        {
            get
            {
                return new[]
                {
                    new IntegrationLinkSetting() { ActionstepOrgKey = ActionstepDefaults.AllOrgsKey , UserId = WCAUser.AllUsersId , IntegrationLinkId = CreateWorkspaceIntegrationLinkId      , SortOrder = 10  , Id = new Guid("B03E1C2C-0F25-47F6-857F-C29A7CEE1F4A")  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLinkSetting() { ActionstepOrgKey = ActionstepDefaults.AllOrgsKey , UserId = WCAUser.AllUsersId , IntegrationLinkId = PropertyInquiryIntegrationLinkId      , SortOrder = 10  , Id = new Guid("98C2C667-7FCF-4B35-BE84-10E7DCF85311")  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLinkSetting() { ActionstepOrgKey = ActionstepDefaults.AllOrgsKey , UserId = WCAUser.AllUsersId , IntegrationLinkId = TitleSearchIntegrationLinkId          , SortOrder = 20  , Id = new Guid("A0910127-ADC7-43DD-958C-9801B7AF09D8")  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLinkSetting() { ActionstepOrgKey = ActionstepDefaults.AllOrgsKey , UserId = WCAUser.AllUsersId , IntegrationLinkId = OrderHistoryIntegrationLinkId         , SortOrder = 30  , Id = new Guid("523D34F6-6151-4B2E-834C-7B1A34FBD6BF")  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLinkSetting() { ActionstepOrgKey = ActionstepDefaults.AllOrgsKey , UserId = WCAUser.AllUsersId , IntegrationLinkId = SettlementCalculatorIntegrationLinkId , SortOrder = 10  , Id = new Guid("0BCD13C4-8320-464D-BD12-960C5AEA1A18")  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                    new IntegrationLinkSetting() { ActionstepOrgKey = ActionstepDefaults.AllOrgsKey , UserId = WCAUser.AllUsersId , IntegrationLinkId = StampDutyCalculatorIntegrationLinkId  , SortOrder = 20  , Id = new Guid("E35020DC-C06C-49C6-8328-EA2B07BDA219")  , DateCreatedUtc = DateTime.MinValue  , LastUpdatedUtc = DateTime.MinValue }  ,
                };
            }
        }
    }
}