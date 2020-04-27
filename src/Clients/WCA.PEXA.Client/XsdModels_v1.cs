using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WCA.PEXA.Client
{
    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address2Type", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address2Type
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("StreetAddress", Namespace = "http://api.pexa.net.au/schema/1/")]
        public Address2StreetAddressDetailsType StreetAddress { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("OverseasAddress", Namespace = "http://api.pexa.net.au/schema/1/")]
        public Address2OverseasAddressDetailsType OverseasAddress { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("CorrespondenceAddress", Namespace = "http://api.pexa.net.au/schema/1/")]
        public Address2CorrespondenceAddressDetailsType CorrespondenceAddress { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("businessType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class BusinessType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("LegalEntityName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string LegalEntityName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("BusinessName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string BusinessName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("BusinessUnit", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string BusinessUnit { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("OrganisationType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string OrganisationType { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<AdministrationStatusDetailsType> _administrationStatus;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("AdministrationStatus", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<AdministrationStatusDetailsType> AdministrationStatus
        {
            get
            {
                return this._administrationStatus;
            }
            private set
            {
                this._administrationStatus = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die AdministrationStatus-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the AdministrationStatus collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AdministrationStatusSpecified
        {
            get
            {
                return (this.AdministrationStatus.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="BusinessType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="BusinessType" /> class.</para>
        /// </summary>
        public BusinessType()
        {
            this._administrationStatus = new System.Collections.ObjectModel.Collection<AdministrationStatusDetailsType>();
            this._identification = new System.Collections.ObjectModel.Collection<IdentificationDetailsType>();
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<IdentificationDetailsType> _identification;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Identification", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<IdentificationDetailsType> Identification
        {
            get
            {
                return this._identification;
            }
            private set
            {
                this._identification = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Identification-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Identification collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdentificationSpecified
        {
            get
            {
                return (this.Identification.Count != 0);
            }
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("fullNameType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class FullNameType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("NameTitle", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string NameTitle { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<GivenNameOrderType> _givenName;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("GivenName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<GivenNameOrderType> GivenName
        {
            get
            {
                return this._givenName;
            }
            private set
            {
                this._givenName = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="FullNameType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="FullNameType" /> class.</para>
        /// </summary>
        public FullNameType()
        {
            this._givenName = new System.Collections.ObjectModel.Collection<GivenNameOrderType>();
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("FamilyName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string FamilyName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("FamilyNameOrder", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string FamilyNameOrder { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("NameSuffix", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string NameSuffix { get; set; }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("DateOfBirth", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "date")]
        public System.DateTime DateOfBirthValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die DateOfBirth-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the DateOfBirth property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool DateOfBirthValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> DateOfBirth
        {
            get
            {
                if (this.DateOfBirthValueSpecified)
                {
                    return this.DateOfBirthValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.DateOfBirthValue = value.GetValueOrDefault();
                this.DateOfBirthValueSpecified = value.HasValue;
            }
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("keyValueType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class KeyValueType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Key", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Key { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Value", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Value { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("primaryLandUseDetailType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class PrimaryLandUseDetailType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("Key", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Key { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("Value", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Value { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("currentLandUseDetailType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class CurrentLandUseDetailType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("VacantLandUse", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string VacantLandUse { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Dwelling", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Dwelling { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("MultiUnit", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string MultiUnit { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Flats", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Flats { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("GuestHousePrivateHotel", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string GuestHousePrivateHotel { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Farming", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Farming { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Industrial", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Industrial { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Commercial", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Commercial { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("Other", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Other { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("constructionTypeDetailType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class ConstructionTypeDetailType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("Key", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Key { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("Value", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Value { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("areaOfLandDetailType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class AreaOfLandDetailType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Area", Namespace = "http://api.pexa.net.au/schema/1/")]
        public decimal Area { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Measurement", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Measurement { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("safetySwitchDetailType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class SafetySwitchDetailType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SafetySwitchInstalled", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SafetySwitchInstalled { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SafetySwitchInformed", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SafetySwitchInformed { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("smokeAlarmDetailType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class SmokeAlarmDetailType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SmokeAlarmInstalled", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SmokeAlarmInstalled { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SmokeAlarmInformed", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SmokeAlarmInformed { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("createDischargeOfMortgageRequestType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("CreateDischargeOfMortgageRequest", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class CreateDischargeOfMortgageRequestType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Role { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _mortgageDetails;

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlArrayAttribute("MortgageDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("MortgageNumber", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<string> MortgageDetails
        {
            get
            {
                return this._mortgageDetails;
            }
            private set
            {
                this._mortgageDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="CreateDischargeOfMortgageRequestType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="CreateDischargeOfMortgageRequestType" /> class.</para>
        /// </summary>
        public CreateDischargeOfMortgageRequestType()
        {
            this._mortgageDetails = new System.Collections.ObjectModel.Collection<string>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("createDischargeOfMortgageResponseType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("CreateDischargeOfMortgageResponse", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class CreateDischargeOfMortgageResponseType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("DocumentId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string DocumentId { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("createProjectParticipationRequestType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("CreateProjectParticipationRequest", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class CreateProjectParticipationRequestType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("ProjectsId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ProjectsId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ParticipantDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public ProjectParticipantDetailsDetailType ParticipantDetails { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ParticipantRole", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ParticipantRole { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PartyName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PartyName { get; set; }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("EstimatedSettlementDate", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "date")]
        public System.DateTime EstimatedSettlementDateValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die EstimatedSettlementDate-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the EstimatedSettlementDate property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool EstimatedSettlementDateValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> EstimatedSettlementDate
        {
            get
            {
                if (this.EstimatedSettlementDateValueSpecified)
                {
                    return this.EstimatedSettlementDateValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.EstimatedSettlementDateValue = value.GetValueOrDefault();
                this.EstimatedSettlementDateValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Notes", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Notes { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("projectParticipantDetailsDetailType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class ProjectParticipantDetailsDetailType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberId { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("createWorkspaceInvitationRequestType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("CreateWorkspaceInvitationRequest", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class CreateWorkspaceInvitationRequestType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ParticipantDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public ParticipantDetailsDetailType ParticipantDetails { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ParticipantRole", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ParticipantRole { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Notes", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Notes { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("participantDetailsDetailType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class ParticipantDetailsDetailType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberId { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("createWorkspaceInvitationResponseType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("CreateWorkspaceInvitationResponse", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class CreateWorkspaceInvitationResponseType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("InviteId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string InviteId { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceUpdateRequestType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("WorkspaceUpdateRequest", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class WorkspaceUpdateRequestType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Role { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SubscriberReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberReference { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceUpdateRequestTypeLandTitleDetailsLandTitle> _landTitleDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("LandTitleDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("LandTitle", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<WorkspaceUpdateRequestTypeLandTitleDetailsLandTitle> LandTitleDetails
        {
            get
            {
                return this._landTitleDetails;
            }
            private set
            {
                this._landTitleDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die LandTitleDetails-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the LandTitleDetails collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LandTitleDetailsSpecified
        {
            get
            {
                return (this.LandTitleDetails.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceUpdateRequestType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceUpdateRequestType" /> class.</para>
        /// </summary>
        public WorkspaceUpdateRequestType()
        {
            this._landTitleDetails = new System.Collections.ObjectModel.Collection<WorkspaceUpdateRequestTypeLandTitleDetailsLandTitle>();
            this._additionalRoleDetails = new System.Collections.ObjectModel.Collection<WorkspaceUpdateRequestTypeAdditionalRoleDetailsOtherRole>();
            this._partyDetails = new System.Collections.ObjectModel.Collection<WorkspaceUpdateRequestTypePartyDetailsParty>();
        }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("NewSubscriberReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string NewSubscriberReference { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceUpdateRequestTypeAdditionalRoleDetailsOtherRole> _additionalRoleDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("AdditionalRoleDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("OtherRole", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<WorkspaceUpdateRequestTypeAdditionalRoleDetailsOtherRole> AdditionalRoleDetails
        {
            get
            {
                return this._additionalRoleDetails;
            }
            private set
            {
                this._additionalRoleDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die AdditionalRoleDetails-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the AdditionalRoleDetails collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AdditionalRoleDetailsSpecified
        {
            get
            {
                return (this.AdditionalRoleDetails.Count != 0);
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceUpdateRequestTypePartyDetailsParty> _partyDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("PartyDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Party", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<WorkspaceUpdateRequestTypePartyDetailsParty> PartyDetails
        {
            get
            {
                return this._partyDetails;
            }
            private set
            {
                this._partyDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die PartyDetails-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the PartyDetails collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PartyDetailsSpecified
        {
            get
            {
                return (this.PartyDetails.Count != 0);
            }
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceUpdateResponseType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("WorkspaceUpdateResponse", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class WorkspaceUpdateResponseType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceUpdateResponseTypeLandTitleDetailsLandTitle> _landTitleDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("LandTitleDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("LandTitle", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<WorkspaceUpdateResponseTypeLandTitleDetailsLandTitle> LandTitleDetails
        {
            get
            {
                return this._landTitleDetails;
            }
            private set
            {
                this._landTitleDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die LandTitleDetails-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the LandTitleDetails collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LandTitleDetailsSpecified
        {
            get
            {
                return (this.LandTitleDetails.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceUpdateResponseType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceUpdateResponseType" /> class.</para>
        /// </summary>
        public WorkspaceUpdateResponseType()
        {
            this._landTitleDetails = new System.Collections.ObjectModel.Collection<WorkspaceUpdateResponseTypeLandTitleDetailsLandTitle>();
            this._partyDetails = new System.Collections.ObjectModel.Collection<WorkspaceUpdateResponseTypePartyDetailsParty>();
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceStatus", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceStatus { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceUpdateResponseTypePartyDetailsParty> _partyDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("PartyDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Party", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<WorkspaceUpdateResponseTypePartyDetailsParty> PartyDetails
        {
            get
            {
                return this._partyDetails;
            }
            private set
            {
                this._partyDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die PartyDetails-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the PartyDetails collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PartyDetailsSpecified
        {
            get
            {
                return (this.PartyDetails.Count != 0);
            }
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceUpdateRequestTypeLandTitleDetailsLandTitle", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceUpdateRequestTypeLandTitleDetailsLandTitle
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("LandTitleActionType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string LandTitleActionType { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("LandTitleReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string LandTitleReference { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceUpdateRequestTypeAdditionalRoleDetailsOtherRole", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceUpdateRequestTypeAdditionalRoleDetailsOtherRole
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("RoleActionType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string RoleActionType { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Role { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SubscriberReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberReference { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("IsDefaultRole", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string IsDefaultRole { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceUpdateRequestTypePartyDetailsParty", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceUpdateRequestTypePartyDetailsParty
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PartyActionType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PartyActionType { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("PartyId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PartyId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("RepresentingParty", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string RepresentingParty { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PartyType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PartyType { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PartyRole", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PartyRole { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("FullName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.FullNameType FullName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Business", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.BusinessType Business { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("CurrentAddress", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.Address2Type CurrentAddress { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("FutureAddress", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.Address2Type FutureAddress { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PartyCapacityDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.PartyCapacityDetailsType PartyCapacityDetails { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("TrustDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.TrustDetailsType TrustDetails { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ForeignPartyDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.ForeignPartyDetailsType ForeignPartyDetails { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceUpdateResponseTypeLandTitleDetailsLandTitle", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceUpdateResponseTypeLandTitleDetailsLandTitle
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ObsoleteLandTitleReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ObsoleteLandTitleReference { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("NewLandTitleReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string NewLandTitleReference { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceUpdateResponseTypePartyDetailsParty", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceUpdateResponseTypePartyDetailsParty
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("PartyId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PartyId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PartyType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PartyType { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PartyRole", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PartyRole { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("FullName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.FullNameType FullName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Business", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.BusinessType Business { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("CurrentAddress", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.Address2Type CurrentAddress { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("FutureAddress", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.Address2Type FutureAddress { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("landTitleReferenceVerificationResponseType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("LandTitleReferenceVerificationResponse", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class LandTitleReferenceVerificationResponseType
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<LandTitleReferenceVerificationResponseTypeWarningsWorkspace> _warnings;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("Warnings", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Workspace", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<LandTitleReferenceVerificationResponseTypeWarningsWorkspace> Warnings
        {
            get
            {
                return this._warnings;
            }
            private set
            {
                this._warnings = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Warnings-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Warnings collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WarningsSpecified
        {
            get
            {
                return (this.Warnings.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="LandTitleReferenceVerificationResponseType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="LandTitleReferenceVerificationResponseType" /> class.</para>
        /// </summary>
        public LandTitleReferenceVerificationResponseType()
        {
            this._warnings = new System.Collections.ObjectModel.Collection<LandTitleReferenceVerificationResponseTypeWarningsWorkspace>();
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("LandTitleReferenceReport", Namespace = "http://api.pexa.net.au/schema/1/")]
        public LandTitleReferenceVerificationResponseTypeLandTitleReferenceReport LandTitleReferenceReport { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("LandTitleReferenceVerificationResponseTypeLandTitleReferenceReport", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class LandTitleReferenceVerificationResponseTypeLandTitleReferenceReport
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("LandTitleReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string LandTitleReference { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("NewLandTitleReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string NewLandTitleReference { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PropertyDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PropertyDetails { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ElectronicLodgement", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ElectronicLodgement { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _note;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Note", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<string> Note
        {
            get
            {
                return this._note;
            }
            private set
            {
                this._note = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Note-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Note collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool NoteSpecified
        {
            get
            {
                return (this.Note.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="LandTitleReferenceVerificationResponseTypeLandTitleReferenceReport" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="LandTitleReferenceVerificationResponseTypeLandTitleReferenceReport" /> class.</para>
        /// </summary>
        public LandTitleReferenceVerificationResponseTypeLandTitleReferenceReport()
        {
            this._note = new System.Collections.ObjectModel.Collection<string>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("LandTitleReferenceVerificationResponseTypeWarningsWorkspace", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class LandTitleReferenceVerificationResponseTypeWarningsWorkspace
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<LandTitleReferenceVerificationResponseTypeWarningsWorkspaceParticipantsListParticipants> _participantsList;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("ParticipantsList", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Participants", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<LandTitleReferenceVerificationResponseTypeWarningsWorkspaceParticipantsListParticipants> ParticipantsList
        {
            get
            {
                return this._participantsList;
            }
            private set
            {
                this._participantsList = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="LandTitleReferenceVerificationResponseTypeWarningsWorkspace" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="LandTitleReferenceVerificationResponseTypeWarningsWorkspace" /> class.</para>
        /// </summary>
        public LandTitleReferenceVerificationResponseTypeWarningsWorkspace()
        {
            this._participantsList = new System.Collections.ObjectModel.Collection<LandTitleReferenceVerificationResponseTypeWarningsWorkspaceParticipantsListParticipants>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("LandTitleReferenceVerificationResponseTypeWarningsWorkspaceParticipantsListPartic" +
        "ipants", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class LandTitleReferenceVerificationResponseTypeWarningsWorkspaceParticipantsListParticipants
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SubscriberName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Role { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ContactName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ContactName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Email", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Email { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Phone", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Phone { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("maintainSettlementDateAndTimeRequestType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("MaintainSettlementDateAndTimeRequest", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class MaintainSettlementDateAndTimeRequestType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Role { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SettlementActionType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SettlementActionType { get; set; }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("SettlementDateAndTime", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.DateTime SettlementDateAndTimeValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die SettlementDateAndTime-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the SettlementDateAndTime property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool SettlementDateAndTimeValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> SettlementDateAndTime
        {
            get
            {
                if (this.SettlementDateAndTimeValueSpecified)
                {
                    return this.SettlementDateAndTimeValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.SettlementDateAndTimeValue = value.GetValueOrDefault();
                this.SettlementDateAndTimeValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SettlementDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public SettlementDetailsType SettlementDetails { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("settlementDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class SettlementDetailsType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SettlementDateAndTime", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.DateTime SettlementDateAndTime { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ParticipantSettlementAcceptanceStatus", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ParticipantSettlementAcceptanceStatus { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Reason", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Reason { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("manageWorkspaceDataRequestType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ManageWorkspaceDataRequest", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class ManageWorkspaceDataRequestType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Role { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceData", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WorkspaceDataDetailType WorkspaceData { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceDataDetailType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceDataDetailType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("TransactionDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public TransactionDetailsDetailType TransactionDetails { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PropertyDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public PropertyDetailsDetailType PropertyDetails { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("transactionDetailsDetailType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class TransactionDetailsDetailType
    {

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("ConsiderationAmount", Namespace = "http://api.pexa.net.au/schema/1/")]
        public decimal ConsiderationAmountValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die ConsiderationAmount-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the ConsiderationAmount property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ConsiderationAmountValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<decimal> ConsiderationAmount
        {
            get
            {
                if (this.ConsiderationAmountValueSpecified)
                {
                    return this.ConsiderationAmountValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.ConsiderationAmountValue = value.GetValueOrDefault();
                this.ConsiderationAmountValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("ContractOfSaleDate", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "date")]
        public System.DateTime ContractOfSaleDateValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die ContractOfSaleDate-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the ContractOfSaleDate property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool ContractOfSaleDateValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> ContractOfSaleDate
        {
            get
            {
                if (this.ContractOfSaleDateValueSpecified)
                {
                    return this.ContractOfSaleDateValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.ContractOfSaleDateValue = value.GetValueOrDefault();
                this.ContractOfSaleDateValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ConsiderationLessThanValue", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ConsiderationLessThanValue { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("OtherDutiableTransactions", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string OtherDutiableTransactions { get; set; }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("GstAmount", Namespace = "http://api.pexa.net.au/schema/1/")]
        public decimal GstAmountValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die GstAmount-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the GstAmount property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool GstAmountValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<decimal> GstAmount
        {
            get
            {
                if (this.GstAmountValueSpecified)
                {
                    return this.GstAmountValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.GstAmountValue = value.GetValueOrDefault();
                this.GstAmountValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("CovenantMcpReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string CovenantMcpReference { get; set; }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("CovenantMcpExpiryDate", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "date")]
        public System.DateTime CovenantMcpExpiryDateValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die CovenantMcpExpiryDate-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the CovenantMcpExpiryDate property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool CovenantMcpExpiryDateValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> CovenantMcpExpiryDate
        {
            get
            {
                if (this.CovenantMcpExpiryDateValueSpecified)
                {
                    return this.CovenantMcpExpiryDateValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.CovenantMcpExpiryDateValue = value.GetValueOrDefault();
                this.CovenantMcpExpiryDateValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("DepositPaid", Namespace = "http://api.pexa.net.au/schema/1/")]
        public decimal DepositPaidValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die DepositPaid-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the DepositPaid property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool DepositPaidValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<decimal> DepositPaid
        {
            get
            {
                if (this.DepositPaidValueSpecified)
                {
                    return this.DepositPaidValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.DepositPaidValue = value.GetValueOrDefault();
                this.DepositPaidValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PhoneNumber", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("DateOfPossession", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "date")]
        public System.DateTime DateOfPossessionValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die DateOfPossession-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the DateOfPossession property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool DateOfPossessionValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> DateOfPossession
        {
            get
            {
                if (this.DateOfPossessionValueSpecified)
                {
                    return this.DateOfPossessionValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.DateOfPossessionValue = value.GetValueOrDefault();
                this.DateOfPossessionValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("MarginScheme", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string MarginScheme { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("TermsSaleIndicator", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string TermsSaleIndicator { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("propertyDetailsDetailType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class PropertyDetailsDetailType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("NatureOfProperty", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string NatureOfProperty { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PrimaryLandUse", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.PrimaryLandUseDetailType PrimaryLandUse { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("CurrentLandUse", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.CurrentLandUseDetailType CurrentLandUse { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ConstructionType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.ConstructionTypeDetailType ConstructionType { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("MunicipalityName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string MunicipalityName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("LocalityName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string LocalityName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("AreaOfLand", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.AreaOfLandDetailType AreaOfLand { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("NumberOfBedrooms", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string NumberOfBedrooms { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SafetySwitch", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.SafetySwitchDetailType SafetySwitch { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SmokeAlarm", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.SmokeAlarmDetailType SmokeAlarm { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("manageProjectDataRequestType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ManageProjectDataRequest", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class ManageProjectDataRequestType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("ProjectsId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ProjectsId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("ProjectsName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ProjectsName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Role { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("RestrictAccounts", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string RestrictAccounts { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<ManageProjectDataRequestTypeAccountDetailsProjectAccount> _accountDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("AccountDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("ProjectAccount", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<ManageProjectDataRequestTypeAccountDetailsProjectAccount> AccountDetails
        {
            get
            {
                return this._accountDetails;
            }
            private set
            {
                this._accountDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die AccountDetails-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the AccountDetails collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AccountDetailsSpecified
        {
            get
            {
                return (this.AccountDetails.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="ManageProjectDataRequestType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ManageProjectDataRequestType" /> class.</para>
        /// </summary>
        public ManageProjectDataRequestType()
        {
            this._accountDetails = new System.Collections.ObjectModel.Collection<ManageProjectDataRequestTypeAccountDetailsProjectAccount>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("ManageProjectDataRequestTypeAccountDetailsProjectAccount", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class ManageProjectDataRequestTypeAccountDetailsProjectAccount
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Account", Namespace = "http://api.pexa.net.au/schema/1/")]
        public ManageProjectDataRequestTypeAccountDetailsProjectAccountAccount Account { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("BPay", Namespace = "http://api.pexa.net.au/schema/1/")]
        public ManageProjectDataRequestTypeAccountDetailsProjectAccountBPay BPay { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<ManageProjectDataRequestTypeAccountDetailsProjectAccountCategoryDetailsCategory> _categoryDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("CategoryDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Category", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<ManageProjectDataRequestTypeAccountDetailsProjectAccountCategoryDetailsCategory> CategoryDetails
        {
            get
            {
                return this._categoryDetails;
            }
            private set
            {
                this._categoryDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="ManageProjectDataRequestTypeAccountDetailsProjectAccount" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ManageProjectDataRequestTypeAccountDetailsProjectAccount" /> class.</para>
        /// </summary>
        public ManageProjectDataRequestTypeAccountDetailsProjectAccount()
        {
            this._categoryDetails = new System.Collections.ObjectModel.Collection<ManageProjectDataRequestTypeAccountDetailsProjectAccountCategoryDetailsCategory>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("ManageProjectDataRequestTypeAccountDetailsProjectAccountAccount", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class ManageProjectDataRequestTypeAccountDetailsProjectAccountAccount
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("AccountName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string AccountName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("AccountBsb", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string AccountBsb { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("AccountNumber", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("AccountDescription", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string AccountDescription { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("ManageProjectDataRequestTypeAccountDetailsProjectAccountBPay", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class ManageProjectDataRequestTypeAccountDetailsProjectAccountBPay
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("BillerCode", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string BillerCode { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("Reference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Reference { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("ManageProjectDataRequestTypeAccountDetailsProjectAccountCategoryDetailsCategory", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class ManageProjectDataRequestTypeAccountDetailsProjectAccountCategoryDetailsCategory
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Type", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Category", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Category { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("manageTransferDutyRequestType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("ManageTransferDutyRequest", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class ManageTransferDutyRequestType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Role { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<DutyDataDetailType> _transferDutyDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("TransferDutyDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("DutyData", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<DutyDataDetailType> TransferDutyDetails
        {
            get
            {
                return this._transferDutyDetails;
            }
            private set
            {
                this._transferDutyDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die TransferDutyDetails-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the TransferDutyDetails collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TransferDutyDetailsSpecified
        {
            get
            {
                return (this.TransferDutyDetails.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="ManageTransferDutyRequestType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ManageTransferDutyRequestType" /> class.</para>
        /// </summary>
        public ManageTransferDutyRequestType()
        {
            this._transferDutyDetails = new System.Collections.ObjectModel.Collection<DutyDataDetailType>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("dutyDataDetailType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class DutyDataDetailType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("DocumentId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string DocumentId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("RevenueOfficeTransactionId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string RevenueOfficeTransactionId { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceSummaryResponseType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("WorkspaceSummaryResponse", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class WorkspaceSummaryResponseType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Role { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Jurisdiction", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Jurisdiction { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Status", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Status { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceSummaryLandTitlesType> _landTitleDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("LandTitleDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("LandTitle", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<WorkspaceSummaryLandTitlesType> LandTitleDetails
        {
            get
            {
                return this._landTitleDetails;
            }
            private set
            {
                this._landTitleDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceSummaryResponseType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceSummaryResponseType" /> class.</para>
        /// </summary>
        public WorkspaceSummaryResponseType()
        {
            this._landTitleDetails = new System.Collections.ObjectModel.Collection<WorkspaceSummaryLandTitlesType>();
            this._participantDetails = new System.Collections.ObjectModel.Collection<WorkspaceSummaryParticipantType>();
            this._partyDetails = new System.Collections.ObjectModel.Collection<PartyDetailsType>();
            this._documentSummary = new System.Collections.ObjectModel.Collection<LodgementCaseDetailsType>();
            this._taskList = new System.Collections.ObjectModel.Collection<TaskDetailsType>();
            this._fees = new System.Collections.ObjectModel.Collection<FeeItemDetailsType>();
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceSummaryParticipantType> _participantDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("ParticipantDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Participant", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<WorkspaceSummaryParticipantType> ParticipantDetails
        {
            get
            {
                return this._participantDetails;
            }
            private set
            {
                this._participantDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die ParticipantDetails-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the ParticipantDetails collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ParticipantDetailsSpecified
        {
            get
            {
                return (this.ParticipantDetails.Count != 0);
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<PartyDetailsType> _partyDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("PartyDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Party", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<PartyDetailsType> PartyDetails
        {
            get
            {
                return this._partyDetails;
            }
            private set
            {
                this._partyDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die PartyDetails-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the PartyDetails collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PartyDetailsSpecified
        {
            get
            {
                return (this.PartyDetails.Count != 0);
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<LodgementCaseDetailsType> _documentSummary;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("DocumentSummary", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("LodgementCase", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<LodgementCaseDetailsType> DocumentSummary
        {
            get
            {
                return this._documentSummary;
            }
            private set
            {
                this._documentSummary = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die DocumentSummary-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the DocumentSummary collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DocumentSummarySpecified
        {
            get
            {
                return (this.DocumentSummary.Count != 0);
            }
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SettlementSchedule", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WorkspaceSummarySettlementScheduleDetailsType SettlementSchedule { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("BusinessEventsList", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WorkspaceSummaryBusinessEventsListDetailsType BusinessEventsList { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TaskDetailsType> _taskList;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("TaskList", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Task", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<TaskDetailsType> TaskList
        {
            get
            {
                return this._taskList;
            }
            private set
            {
                this._taskList = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die TaskList-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the TaskList collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TaskListSpecified
        {
            get
            {
                return (this.TaskList.Count != 0);
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<FeeItemDetailsType> _fees;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("Fees", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("FeeItem", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<FeeItemDetailsType> Fees
        {
            get
            {
                return this._fees;
            }
            private set
            {
                this._fees = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Fees-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Fees collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool FeesSpecified
        {
            get
            {
                return (this.Fees.Count != 0);
            }
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceSummarySettlementScheduleDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceSummarySettlementScheduleDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("SettlementScheduleId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SettlementScheduleId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Status", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Status { get; set; }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("SettlementDate", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.DateTime SettlementDateValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die SettlementDate-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the SettlementDate property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool SettlementDateValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> SettlementDate
        {
            get
            {
                if (this.SettlementDateValueSpecified)
                {
                    return this.SettlementDateValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.SettlementDateValue = value.GetValueOrDefault();
                this.SettlementDateValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("OverallSettlementAcceptanceStatus", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string OverallSettlementAcceptanceStatus { get; set; }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("LastModified", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.DateTime LastModifiedValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die LastModified-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the LastModified property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool LastModifiedValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> LastModified
        {
            get
            {
                if (this.LastModifiedValueSpecified)
                {
                    return this.LastModifiedValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.LastModifiedValue = value.GetValueOrDefault();
                this.LastModifiedValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SignaturesRequired", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SignaturesRequired { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SignaturesReceived", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SignaturesReceived { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceSummaryLandTitlesType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceSummaryLandTitlesType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("LandTitleReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string LandTitleReference { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ParentTitle", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ParentTitle { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PropertyDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PropertyDetails { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceSummaryParticipantType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceSummaryParticipantType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SubscriberName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberName { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceSummaryWorkspaceDetailsType> _workspace;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Workspace", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<WorkspaceSummaryWorkspaceDetailsType> Workspace
        {
            get
            {
                return this._workspace;
            }
            private set
            {
                this._workspace = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceSummaryParticipantType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceSummaryParticipantType" /> class.</para>
        /// </summary>
        public WorkspaceSummaryParticipantType()
        {
            this._workspace = new System.Collections.ObjectModel.Collection<WorkspaceSummaryWorkspaceDetailsType>();
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ContactDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WorkspacesummaryParticipantContactDetailsType ContactDetails { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceSummaryWorkspaceDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceSummaryWorkspaceDetailsType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Role { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SubscriberReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberReference { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Status", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Status { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceSummaryPartyType> _representedParty;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("RepresentedParty", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Party", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<WorkspaceSummaryPartyType> RepresentedParty
        {
            get
            {
                return this._representedParty;
            }
            private set
            {
                this._representedParty = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die RepresentedParty-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the RepresentedParty collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool RepresentedPartySpecified
        {
            get
            {
                return (this.RepresentedParty.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceSummaryWorkspaceDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceSummaryWorkspaceDetailsType" /> class.</para>
        /// </summary>
        public WorkspaceSummaryWorkspaceDetailsType()
        {
            this._representedParty = new System.Collections.ObjectModel.Collection<WorkspaceSummaryPartyType>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspacesummaryParticipantContactDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspacesummaryParticipantContactDetailsType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Email", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Email { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Phone", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Phone { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceSummaryPartyType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceSummaryPartyType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("PartyId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PartyId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("RepresentingSelf", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string RepresentingSelf { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("partyDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class PartyDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("PartyId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PartyId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PartyRole", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PartyRole { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("UnrepresentedParty", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string UnrepresentedParty { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PartyType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PartyType { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("FullName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.FullNameType FullName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Business", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.BusinessType Business { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("CurrentAddress", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.Address1Type CurrentAddress { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("FutureAddress", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.Address1Type FutureAddress { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PartyCapacityDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.PartyCapacityDetailsType PartyCapacityDetails { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("TrustDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.TrustDetailsType TrustDetails { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ForeignPartyDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.ForeignPartyDetailsType ForeignPartyDetails { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("lodgementCaseDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class LodgementCaseDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("LodgementCaseId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string LodgementCaseId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("LodgementCaseStatus", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string LodgementCaseStatus { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<DocumentDetailsType> _documents;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("Documents", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Document", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<DocumentDetailsType> Documents
        {
            get
            {
                return this._documents;
            }
            private set
            {
                this._documents = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="LodgementCaseDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="LodgementCaseDetailsType" /> class.</para>
        /// </summary>
        public LodgementCaseDetailsType()
        {
            this._documents = new System.Collections.ObjectModel.Collection<DocumentDetailsType>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceSummaryBusinessEventsListDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceSummaryBusinessEventsListDetailsType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Count", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Count { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("MoreData", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string MoreData { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<EventDetailsType> _event;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Event", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<EventDetailsType> Event
        {
            get
            {
                return this._event;
            }
            private set
            {
                this._event = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Event-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Event collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool EventSpecified
        {
            get
            {
                return (this.Event.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceSummaryBusinessEventsListDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceSummaryBusinessEventsListDetailsType" /> class.</para>
        /// </summary>
        public WorkspaceSummaryBusinessEventsListDetailsType()
        {
            this._event = new System.Collections.ObjectModel.Collection<EventDetailsType>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("documentDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class DocumentDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("DocumentId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string DocumentId { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _landTitleReference;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("LandTitleReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<string> LandTitleReference
        {
            get
            {
                return this._landTitleReference;
            }
            private set
            {
                this._landTitleReference = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="DocumentDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="DocumentDetailsType" /> class.</para>
        /// </summary>
        public DocumentDetailsType()
        {
            this._landTitleReference = new System.Collections.ObjectModel.Collection<string>();
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("DocumentStatus", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string DocumentStatus { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("DocumentType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string DocumentType { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("RelatedDocumentId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string RelatedDocumentId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("LastModified", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.DateTime LastModified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SignaturesRequired", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SignaturesRequired { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SignaturesReceived", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SignaturesReceived { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("LodgementVerification", Namespace = "http://api.pexa.net.au/schema/1/")]
        public LodgementVerificationDetailsType LodgementVerification { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("StampDuty", Namespace = "http://api.pexa.net.au/schema/1/")]
        public StampDutyDetailsType StampDuty { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Lodgement", Namespace = "http://api.pexa.net.au/schema/1/")]
        public LodgementDetailsType Lodgement { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("lodgementVerificationDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class LodgementVerificationDetailsType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("VerificationResult", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string VerificationResult { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Date", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.DateTime Date { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("stampDutyDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class StampDutyDetailsType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("VerificationResult", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string VerificationResult { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Date", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.DateTime Date { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("lodgementDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class LodgementDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("DealingNumber", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string DealingNumber { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Date", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.DateTime Date { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("eventDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class EventDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("EventId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string EventId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Date", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.DateTime Date { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Severity", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Severity { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Category", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Category { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Type", Namespace = "http://api.pexa.net.au/schema/1/")]
        public TypeDetailsType Type { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Detail", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Detail { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("typeDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class TypeDetailsType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Key", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Key { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Value", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Value { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("taskDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class TaskDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("TaskId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string TaskId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("DateCreated", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.DateTime DateCreated { get; set; }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("DateCompleted", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.DateTime DateCompletedValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die DateCompleted-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the DateCompleted property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool DateCompletedValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> DateCompleted
        {
            get
            {
                if (this.DateCompletedValueSpecified)
                {
                    return this.DateCompletedValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.DateCompletedValue = value.GetValueOrDefault();
                this.DateCompletedValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Type", Namespace = "http://api.pexa.net.au/schema/1/")]
        public TypeDetailsType Type { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Status", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Status { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("feeItemDetailsType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    public partial class FeeItemDetailsType
    {

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("Date", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.DateTime DateValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Date-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Date property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool DateValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> Date
        {
            get
            {
                if (this.DateValueSpecified)
                {
                    return this.DateValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.DateValue = value.GetValueOrDefault();
                this.DateValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("DocumentType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string DocumentType { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Type", Namespace = "http://api.pexa.net.au/schema/1/")]
        public TypeDetailsType Type { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _signingSubscriber;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("SigningSubscriber", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("SubscriberName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<string> SigningSubscriber
        {
            get
            {
                return this._signingSubscriber;
            }
            private set
            {
                this._signingSubscriber = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die SigningSubscriber-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the SigningSubscriber collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SigningSubscriberSpecified
        {
            get
            {
                return (this.SigningSubscriber.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="FeeItemDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="FeeItemDetailsType" /> class.</para>
        /// </summary>
        public FeeItemDetailsType()
        {
            this._signingSubscriber = new System.Collections.ObjectModel.Collection<string>();
        }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("NetAmount", Namespace = "http://api.pexa.net.au/schema/1/")]
        public decimal NetAmountValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die NetAmount-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the NetAmount property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool NetAmountValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<decimal> NetAmount
        {
            get
            {
                if (this.NetAmountValueSpecified)
                {
                    return this.NetAmountValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.NetAmountValue = value.GetValueOrDefault();
                this.NetAmountValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Xml.Serialization.XmlElementAttribute("GstAmount", Namespace = "http://api.pexa.net.au/schema/1/")]
        public decimal GstAmountValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die GstAmount-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the GstAmount property is specified.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        public bool GstAmountValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public System.Nullable<decimal> GstAmount
        {
            get
            {
                if (this.GstAmountValueSpecified)
                {
                    return this.GstAmountValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.GstAmountValue = value.GetValueOrDefault();
                this.GstAmountValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("TotalAmount", Namespace = "http://api.pexa.net.au/schema/1/")]
        public decimal TotalAmount { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("respondInvitationsRequestType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("RespondInvitationRequest", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class RespondInvitationsRequestType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("InviteId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string InviteId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("InvitationResponse", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string InvitationResponse { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ResponseReason", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ResponseReason { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("AdditionalText", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string AdditionalText { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SubscriberReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberReference { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("ProjectId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ProjectId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ProjectName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ProjectName { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("settlementUploadRequestType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("SettlementUploadRequest", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class SettlementUploadRequestType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Role { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<SettlementUploadRequestTypeScheduleLineItem> _schedule;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("Schedule", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("LineItem", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<SettlementUploadRequestTypeScheduleLineItem> Schedule
        {
            get
            {
                return this._schedule;
            }
            private set
            {
                this._schedule = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="SettlementUploadRequestType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="SettlementUploadRequestType" /> class.</para>
        /// </summary>
        public SettlementUploadRequestType()
        {
            this._schedule = new System.Collections.ObjectModel.Collection<SettlementUploadRequestTypeScheduleLineItem>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("settlementUploadResponseType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("SettlementUploadResponse", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class SettlementUploadResponseType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Warning", Namespace = "http://api.pexa.net.au/schema/1/")]
        public WCA.PEXA.Client.WarningResponseType Warning { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("SettlementUploadRequestTypeScheduleLineItem", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class SettlementUploadRequestTypeScheduleLineItem
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Type", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Category", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Category { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("CustomCategory", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string CustomCategory { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("AccountDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public SettlementUploadRequestTypeScheduleLineItemAccountDetails AccountDetails { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("AssociatedSubscriber", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string AssociatedSubscriber { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("TransactionDescription", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string TransactionDescription { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Value", Namespace = "http://api.pexa.net.au/schema/1/")]
        public decimal Value { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ClientName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ClientName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Verified", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Verified { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("SettlementUploadRequestTypeScheduleLineItemAccountDetails", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class SettlementUploadRequestTypeScheduleLineItemAccountDetails
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Source", Namespace = "http://api.pexa.net.au/schema/1/")]
        public SettlementUploadRequestTypeScheduleLineItemAccountDetailsSource Source { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Destination", Namespace = "http://api.pexa.net.au/schema/1/")]
        public SettlementUploadRequestTypeScheduleLineItemAccountDetailsDestination Destination { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("SettlementUploadRequestTypeScheduleLineItemAccountDetailsSource", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class SettlementUploadRequestTypeScheduleLineItemAccountDetailsSource
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("DefaultAccount", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string DefaultAccount { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("PreverifiedAccount", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PreverifiedAccount { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("SettlementUploadRequestTypeScheduleLineItemAccountDetailsDestination", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class SettlementUploadRequestTypeScheduleLineItemAccountDetailsDestination
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("PreverifiedAccount", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PreverifiedAccount { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("OfficeAccount", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string OfficeAccount { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Account", Namespace = "http://api.pexa.net.au/schema/1/")]
        public SettlementUploadRequestTypeScheduleLineItemAccountDetailsDestinationAccount Account { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("BPay", Namespace = "http://api.pexa.net.au/schema/1/")]
        public SettlementUploadRequestTypeScheduleLineItemAccountDetailsDestinationBPay BPay { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("SettlementUploadRequestTypeScheduleLineItemAccountDetailsDestinationAccount", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class SettlementUploadRequestTypeScheduleLineItemAccountDetailsDestinationAccount
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("AccountName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string AccountName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("AccountBsb", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string AccountBsb { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("AccountNumber", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string AccountNumber { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("SettlementUploadRequestTypeScheduleLineItemAccountDetailsDestinationBPay", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class SettlementUploadRequestTypeScheduleLineItemAccountDetailsDestinationBPay
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("BillerCode", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string BillerCode { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("Reference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Reference { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceDocumentRetrievalResponseType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("WorkspaceDocumentRetrieval", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class WorkspaceDocumentRetrievalResponseType
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceDocumentRetrievalResponseTypeLodgementCase> _lodgementCase;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("LodgementCase", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<WorkspaceDocumentRetrievalResponseTypeLodgementCase> LodgementCase
        {
            get
            {
                return this._lodgementCase;
            }
            private set
            {
                this._lodgementCase = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die LodgementCase-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the LodgementCase collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LodgementCaseSpecified
        {
            get
            {
                return (this.LodgementCase.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceDocumentRetrievalResponseType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceDocumentRetrievalResponseType" /> class.</para>
        /// </summary>
        public WorkspaceDocumentRetrievalResponseType()
        {
            this._lodgementCase = new System.Collections.ObjectModel.Collection<WorkspaceDocumentRetrievalResponseTypeLodgementCase>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceDocumentRetrievalResponseTypeLodgementCase", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceDocumentRetrievalResponseTypeLodgementCase
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("LodgementCaseId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string LodgementCaseId { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceDocumentRetrievalResponseTypeLodgementCaseDocumentsDocument> _documents;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("Documents", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Document", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<WorkspaceDocumentRetrievalResponseTypeLodgementCaseDocumentsDocument> Documents
        {
            get
            {
                return this._documents;
            }
            private set
            {
                this._documents = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceDocumentRetrievalResponseTypeLodgementCase" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceDocumentRetrievalResponseTypeLodgementCase" /> class.</para>
        /// </summary>
        public WorkspaceDocumentRetrievalResponseTypeLodgementCase()
        {
            this._documents = new System.Collections.ObjectModel.Collection<WorkspaceDocumentRetrievalResponseTypeLodgementCaseDocumentsDocument>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceDocumentRetrievalResponseTypeLodgementCaseDocumentsDocument", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceDocumentRetrievalResponseTypeLodgementCaseDocumentsDocument
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("DocumentId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string DocumentId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("LrDocumentId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string LrDocumentId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("DocumentType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string DocumentType { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("MimeType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string MimeType { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("RenderedDocument", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "base64Binary")]
        public byte[] RenderedDocument { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("settlementStatementRetrievalResponseType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("SettlementStatementRetrieval", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class SettlementStatementRetrievalResponseType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Documents", Namespace = "http://api.pexa.net.au/schema/1/")]
        public SettlementStatementRetrievalResponseTypeDocuments Documents { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("SettlementStatementRetrievalResponseTypeDocuments", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class SettlementStatementRetrievalResponseTypeDocuments
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Document", Namespace = "http://api.pexa.net.au/schema/1/")]
        public SettlementStatementRetrievalResponseTypeDocumentsDocument Document { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("SettlementStatementRetrievalResponseTypeDocumentsDocument", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class SettlementStatementRetrievalResponseTypeDocumentsDocument
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("DocumentId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string DocumentId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("MimeType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string MimeType { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("RenderedDocument", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "base64Binary")]
        public byte[] RenderedDocument { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workgroupListRetrievalResponseType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("WorkgroupRetrievalResponse", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class WorkgroupListRetrievalResponseType
    {

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkgroupListRetrievalResponseTypeWorkgroup> _workgroup;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Workgroup", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<WorkgroupListRetrievalResponseTypeWorkgroup> Workgroup
        {
            get
            {
                return this._workgroup;
            }
            private set
            {
                this._workgroup = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Workgroup-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Workgroup collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WorkgroupSpecified
        {
            get
            {
                return (this.Workgroup.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkgroupListRetrievalResponseType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkgroupListRetrievalResponseType" /> class.</para>
        /// </summary>
        public WorkgroupListRetrievalResponseType()
        {
            this._workgroup = new System.Collections.ObjectModel.Collection<WorkgroupListRetrievalResponseTypeWorkgroup>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkgroupListRetrievalResponseTypeWorkgroup", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkgroupListRetrievalResponseTypeWorkgroup
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkgroupId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkgroupId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("WorkgroupName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkgroupName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("IsParent", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string IsParent { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _childWorkgroupList;

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlArrayAttribute("ChildWorkgroupList", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("ChildWorkgroupId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<string> ChildWorkgroupList
        {
            get
            {
                return this._childWorkgroupList;
            }
            private set
            {
                this._childWorkgroupList = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die ChildWorkgroupList-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the ChildWorkgroupList collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ChildWorkgroupListSpecified
        {
            get
            {
                return (this.ChildWorkgroupList.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkgroupListRetrievalResponseTypeWorkgroup" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkgroupListRetrievalResponseTypeWorkgroup" /> class.</para>
        /// </summary>
        public WorkgroupListRetrievalResponseTypeWorkgroup()
        {
            this._childWorkgroupList = new System.Collections.ObjectModel.Collection<string>();
            this._parentWorkgroupList = new System.Collections.ObjectModel.Collection<string>();
            this._assignedUsers = new System.Collections.ObjectModel.Collection<string>();
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _parentWorkgroupList;

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlArrayAttribute("ParentWorkgroupList", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("ParentWorkgroupId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<string> ParentWorkgroupList
        {
            get
            {
                return this._parentWorkgroupList;
            }
            private set
            {
                this._parentWorkgroupList = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die ParentWorkgroupList-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the ParentWorkgroupList collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ParentWorkgroupListSpecified
        {
            get
            {
                return (this.ParentWorkgroupList.Count != 0);
            }
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _assignedUsers;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("AssignedUsers", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Username", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<string> AssignedUsers
        {
            get
            {
                return this._assignedUsers;
            }
            private set
            {
                this._assignedUsers = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die AssignedUsers-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the AssignedUsers collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AssignedUsersSpecified
        {
            get
            {
                return (this.AssignedUsers.Count != 0);
            }
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("userProfileRetrievalResponseType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("UserProfileRetrievalResponse", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class UserProfileRetrievalResponseType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("UserProfile", Namespace = "http://api.pexa.net.au/schema/1/")]
        public UserProfileRetrievalResponseTypeUserProfile UserProfile { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("UserProfileRetrievalResponseTypeUserProfile", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class UserProfileRetrievalResponseTypeUserProfile
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SubscriberName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SubscriberType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberType { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SubscriberDualSignFss", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberDualSignFss { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SubscriberDualSignDoc", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SubscriberDualSignDoc { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("UserId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string UserId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Username", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Username { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("UserType", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string UserType { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("UserFirstName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string UserFirstName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("UserLastName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string UserLastName { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<UserProfileRetrievalResponseTypeUserProfileWorkgroupListWorkgroup> _workgroupList;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("WorkgroupList", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Workgroup", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<UserProfileRetrievalResponseTypeUserProfileWorkgroupListWorkgroup> WorkgroupList
        {
            get
            {
                return this._workgroupList;
            }
            private set
            {
                this._workgroupList = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die WorkgroupList-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the WorkgroupList collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool WorkgroupListSpecified
        {
            get
            {
                return (this.WorkgroupList.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="UserProfileRetrievalResponseTypeUserProfile" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="UserProfileRetrievalResponseTypeUserProfile" /> class.</para>
        /// </summary>
        public UserProfileRetrievalResponseTypeUserProfile()
        {
            this._workgroupList = new System.Collections.ObjectModel.Collection<UserProfileRetrievalResponseTypeUserProfileWorkgroupListWorkgroup>();
            this._userPermissionsList = new System.Collections.ObjectModel.Collection<string>();
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _userPermissionsList;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("UserPermissionsList", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("UserPermission", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<string> UserPermissionsList
        {
            get
            {
                return this._userPermissionsList;
            }
            private set
            {
                this._userPermissionsList = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die UserPermissionsList-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the UserPermissionsList collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool UserPermissionsListSpecified
        {
            get
            {
                return (this.UserPermissionsList.Count != 0);
            }
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("UserProfileRetrievalResponseTypeUserProfileWorkgroupListWorkgroup", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class UserProfileRetrievalResponseTypeUserProfileWorkgroupListWorkgroup
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkgroupId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkgroupId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("WorkgroupName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkgroupName { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("invitationRetrievalResponseType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("InvitationRetrievalResponse", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class InvitationRetrievalResponseType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("MoreData", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string MoreData { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<InvitationRetrievalResponseTypeInvite> _invite;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Invite", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<InvitationRetrievalResponseTypeInvite> Invite
        {
            get
            {
                return this._invite;
            }
            private set
            {
                this._invite = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Invite-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Invite collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool InviteSpecified
        {
            get
            {
                return (this.Invite.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="InvitationRetrievalResponseType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="InvitationRetrievalResponseType" /> class.</para>
        /// </summary>
        public InvitationRetrievalResponseType()
        {
            this._invite = new System.Collections.ObjectModel.Collection<InvitationRetrievalResponseTypeInvite>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("InvitationRetrievalResponseTypeInvite", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class InvitationRetrievalResponseTypeInvite
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("InviteId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string InviteId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("ForwardedBy", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string ForwardedBy { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Date", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "date")]
        public System.DateTime Date { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Status", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Status { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [System.ComponentModel.DataAnnotations.MinLengthAttribute(1)]
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("WorkspaceRole", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string WorkspaceRole { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<InvitationRetrievalResponseTypeInviteLandTitleDetailsLandTitle> _landTitleDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("LandTitleDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("LandTitle", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<InvitationRetrievalResponseTypeInviteLandTitleDetailsLandTitle> LandTitleDetails
        {
            get
            {
                return this._landTitleDetails;
            }
            private set
            {
                this._landTitleDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die LandTitleDetails-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the LandTitleDetails collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool LandTitleDetailsSpecified
        {
            get
            {
                return (this.LandTitleDetails.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="InvitationRetrievalResponseTypeInvite" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="InvitationRetrievalResponseTypeInvite" /> class.</para>
        /// </summary>
        public InvitationRetrievalResponseTypeInvite()
        {
            this._landTitleDetails = new System.Collections.ObjectModel.Collection<InvitationRetrievalResponseTypeInviteLandTitleDetailsLandTitle>();
            this._partyDetails = new System.Collections.ObjectModel.Collection<InvitationRetrievalResponseTypeInvitePartyDetailsParty>();
        }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<InvitationRetrievalResponseTypeInvitePartyDetailsParty> _partyDetails;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("PartyDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Party", Namespace = "http://api.pexa.net.au/schema/1/")]
        public System.Collections.ObjectModel.Collection<InvitationRetrievalResponseTypeInvitePartyDetailsParty> PartyDetails
        {
            get
            {
                return this._partyDetails;
            }
            private set
            {
                this._partyDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die PartyDetails-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the PartyDetails collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool PartyDetailsSpecified
        {
            get
            {
                return (this.PartyDetails.Count != 0);
            }
        }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("InviterSubscriberName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string InviterSubscriberName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("InviteeSubscriberName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string InviteeSubscriberName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("Notes", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string Notes { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("InvitationRetrievalResponseTypeInviteLandTitleDetailsLandTitle", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class InvitationRetrievalResponseTypeInviteLandTitleDetailsLandTitle
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("LandTitleReference", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string LandTitleReference { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("PropertyDetails", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string PropertyDetails { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("InvitationRetrievalResponseTypeInvitePartyDetailsParty", Namespace = "http://api.pexa.net.au/schema/1/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class InvitationRetrievalResponseTypeInvitePartyDetailsParty
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("FullName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string FullName { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("OrganisationName", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string OrganisationName { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("retrieveSettlementAvailabilityResponseType", Namespace = "http://api.pexa.net.au/schema/1/")]
    [DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlRootAttribute("SettlementAvailabilityResponse", Namespace = "http://api.pexa.net.au/schema/1/")]
    public partial class RetrieveSettlementAvailabilityResponseType
    {

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SettlementDate", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "date")]
        public System.DateTime SettlementDate { get; set; }

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlElementAttribute("SettlementAvailability", Namespace = "http://api.pexa.net.au/schema/1/")]
        public string SettlementAvailability { get; set; }

        [System.Xml.Serialization.XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<System.DateTime> _availableSettlementTime;

        /// <summary>
        /// </summary>
        [System.Xml.Serialization.XmlArrayAttribute("AvailableSettlementTime", Namespace = "http://api.pexa.net.au/schema/1/")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Time", Namespace = "http://api.pexa.net.au/schema/1/", DataType = "dateTime")]
        public System.Collections.ObjectModel.Collection<System.DateTime> AvailableSettlementTime
        {
            get
            {
                return this._availableSettlementTime;
            }
            private set
            {
                this._availableSettlementTime = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die AvailableSettlementTime-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the AvailableSettlementTime collection is empty.</para>
        /// </summary>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool AvailableSettlementTimeSpecified
        {
            get
            {
                return (this.AvailableSettlementTime.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="RetrieveSettlementAvailabilityResponseType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="RetrieveSettlementAvailabilityResponseType" /> class.</para>
        /// </summary>
        public RetrieveSettlementAvailabilityResponseType()
        {
            this._availableSettlementTime = new System.Collections.ObjectModel.Collection<System.DateTime>();
        }
    }
}
