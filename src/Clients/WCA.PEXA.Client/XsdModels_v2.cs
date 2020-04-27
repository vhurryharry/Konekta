using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace WCA.PEXA.Client
{
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address1Type", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address1Type
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("StreetAddress", Namespace = "http://api.pexa.net.au/schema/2/")]
        public Address1StreetAddressDetailsType StreetAddress { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("OverseasAddress", Namespace = "http://api.pexa.net.au/schema/2/")]
        public Address1OverseasAddressDetailsType OverseasAddress { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address1StreetAddressDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address1StreetAddressDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SubDwellingUnitType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public Address1StreetAddressSubDwellingUnitTypeDetailsType SubDwellingUnitType { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Level", Namespace = "http://api.pexa.net.au/schema/2/")]
        public Address1StreetAddressLevelDetailsType Level { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<Address1StreetAddressComplexRoadDetailsType> _complexRoad;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ComplexRoad", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<Address1StreetAddressComplexRoadDetailsType> ComplexRoad
        {
            get
            {
                return this._complexRoad;
            }
            private set
            {
                this._complexRoad = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die ComplexRoad-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the ComplexRoad collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool ComplexRoadSpecified
        {
            get
            {
                return (this.ComplexRoad.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="Address1StreetAddressDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="Address1StreetAddressDetailsType" /> class.</para>
        /// </summary>
        public Address1StreetAddressDetailsType()
        {
            this._complexRoad = new System.Collections.ObjectModel.Collection<Address1StreetAddressComplexRoadDetailsType>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SecondaryComplex", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SecondaryComplex { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("AddressSiteName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string AddressSiteName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Road", Namespace = "http://api.pexa.net.au/schema/2/")]
        public Address1StreetAddressRoadDetailsType Road { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LocalityName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LocalityName { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("Postcode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Postcode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("State", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string State { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address1StreetAddressSubDwellingUnitTypeDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address1StreetAddressSubDwellingUnitTypeDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("UnitTypeCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string UnitTypeCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("UnitNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string UnitNumber { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address1StreetAddressLevelDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address1StreetAddressLevelDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LevelTypeCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LevelTypeCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LevelNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LevelNumber { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address1StreetAddressComplexRoadDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address1StreetAddressComplexRoadDetailsType
    {

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<Address1StreetAddressComplexRoadNumberType> _roadNumber;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<Address1StreetAddressComplexRoadNumberType> RoadNumber
        {
            get
            {
                return this._roadNumber;
            }
            private set
            {
                this._roadNumber = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die RoadNumber-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the RoadNumber collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool RoadNumberSpecified
        {
            get
            {
                return (this.RoadNumber.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="Address1StreetAddressComplexRoadDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="Address1StreetAddressComplexRoadDetailsType" /> class.</para>
        /// </summary>
        public Address1StreetAddressComplexRoadDetailsType()
        {
            this._roadNumber = new System.Collections.ObjectModel.Collection<Address1StreetAddressComplexRoadNumberType>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadTypeCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadTypeCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadSuffixCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadSuffixCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlAttributeAttribute("order", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Order { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address1StreetAddressComplexRoadNumberType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address1StreetAddressComplexRoadNumberType
    {

        /// <summary>
        /// <para xml:lang="de">Ruft den Text ab oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets the text value.</para>
        /// </summary>
        [XmlTextAttribute()]
        public string Value { get; set; }

        /// <summary>
        /// </summary>
        [XmlAttributeAttribute("order", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Order { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address1StreetAddressRoadDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address1StreetAddressRoadDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LotNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LotNumber { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("RoadNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadNumber { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadTypeCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadTypeCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadSuffixCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadSuffixCode { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address1OverseasAddressDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address1OverseasAddressDetailsType
    {

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<Address1OverseasAddress1AddressLineDetailsType> _addressLine;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("AddressLine", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<Address1OverseasAddress1AddressLineDetailsType> AddressLine
        {
            get
            {
                return this._addressLine;
            }
            private set
            {
                this._addressLine = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die AddressLine-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the AddressLine collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool AddressLineSpecified
        {
            get
            {
                return (this.AddressLine.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="Address1OverseasAddressDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="Address1OverseasAddressDetailsType" /> class.</para>
        /// </summary>
        public Address1OverseasAddressDetailsType()
        {
            this._addressLine = new System.Collections.ObjectModel.Collection<Address1OverseasAddress1AddressLineDetailsType>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PostalCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PostalCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("CountryCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string CountryCode { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address1OverseasAddress1AddressLineDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address1OverseasAddress1AddressLineDetailsType
    {

        /// <summary>
        /// <para xml:lang="de">Ruft den Text ab oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets the text value.</para>
        /// </summary>
        [XmlTextAttribute()]
        public string Value { get; set; }

        /// <summary>
        /// </summary>
        [XmlAttributeAttribute("order", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Order { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address2Type", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address2
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("StreetAddress", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Address2StreetAddressDetailsType StreetAddress { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("OverseasAddress", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Address2OverseasAddressDetailsType OverseasAddress { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("CorrespondenceAddress", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Address2CorrespondenceAddressDetailsType CorrespondenceAddress { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address2StreetAddressDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address2StreetAddressDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SubDwellingUnitType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public Address2StreetAddressSubDwellingUnitTypeDetailsType SubDwellingUnitType { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Level", Namespace = "http://api.pexa.net.au/schema/2/")]
        public Address2StreetAddressLevelDetailsType Level { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<Address2StreetAddressComplexRoadDetailsType> _complexRoad;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ComplexRoad", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<Address2StreetAddressComplexRoadDetailsType> ComplexRoad
        {
            get
            {
                return this._complexRoad;
            }
            private set
            {
                this._complexRoad = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die ComplexRoad-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the ComplexRoad collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool ComplexRoadSpecified
        {
            get
            {
                return (this.ComplexRoad.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="Address2StreetAddressDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="Address2StreetAddressDetailsType" /> class.</para>
        /// </summary>
        public Address2StreetAddressDetailsType()
        {
            this._complexRoad = new System.Collections.ObjectModel.Collection<Address2StreetAddressComplexRoadDetailsType>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SecondaryComplex", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SecondaryComplex { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("AddressSiteName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string AddressSiteName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Road", Namespace = "http://api.pexa.net.au/schema/2/")]
        public Address2StreetAddressRoadDetailsType Road { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("LocalityName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LocalityName { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("Postcode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Postcode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("State", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string State { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address2StreetAddressSubDwellingUnitTypeDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address2StreetAddressSubDwellingUnitTypeDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("UnitTypeCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string UnitTypeCode { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("UnitNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string UnitNumber { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address2StreetAddressLevelDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address2StreetAddressLevelDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LevelTypeCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LevelTypeCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LevelNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LevelNumber { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address2StreetAddressComplexRoadDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address2StreetAddressComplexRoadDetailsType
    {

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<Address2StreetAddressComplexRoadNumberType> _roadNumber;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<Address2StreetAddressComplexRoadNumberType> RoadNumber
        {
            get
            {
                return this._roadNumber;
            }
            private set
            {
                this._roadNumber = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die RoadNumber-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the RoadNumber collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool RoadNumberSpecified
        {
            get
            {
                return (this.RoadNumber.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="Address2StreetAddressComplexRoadDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="Address2StreetAddressComplexRoadDetailsType" /> class.</para>
        /// </summary>
        public Address2StreetAddressComplexRoadDetailsType()
        {
            this._roadNumber = new System.Collections.ObjectModel.Collection<Address2StreetAddressComplexRoadNumberType>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadTypeCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadTypeCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadSuffixCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadSuffixCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlAttributeAttribute("order", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Order { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address2StreetAddressComplexRoadNumberType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address2StreetAddressComplexRoadNumberType
    {

        /// <summary>
        /// <para xml:lang="de">Ruft den Text ab oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets the text value.</para>
        /// </summary>
        [XmlTextAttribute()]
        public string Value { get; set; }

        /// <summary>
        /// </summary>
        [XmlAttributeAttribute("order", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Order { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address2StreetAddressRoadDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address2StreetAddressRoadDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LotNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LotNumber { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("RoadNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadNumber { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("RoadName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadTypeCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadTypeCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RoadSuffixCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string RoadSuffixCode { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address2OverseasAddressDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address2OverseasAddressDetailsType
    {

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<Address2OverseasAddressAddressLineDetailsType> _addressLine;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("AddressLine", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<Address2OverseasAddressAddressLineDetailsType> AddressLine
        {
            get
            {
                return this._addressLine;
            }
            private set
            {
                this._addressLine = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="Address2OverseasAddressDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="Address2OverseasAddressDetailsType" /> class.</para>
        /// </summary>
        public Address2OverseasAddressDetailsType()
        {
            this._addressLine = new System.Collections.ObjectModel.Collection<Address2OverseasAddressAddressLineDetailsType>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PostalCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PostalCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("CountryCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string CountryCode { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address2OverseasAddressAddressLineDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address2OverseasAddressAddressLineDetailsType
    {

        /// <summary>
        /// <para xml:lang="de">Ruft den Text ab oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets the text value.</para>
        /// </summary>
        [XmlTextAttribute()]
        public string Value { get; set; }

        /// <summary>
        /// </summary>
        [XmlAttributeAttribute("order", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Order { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address2CorrespondenceAddressDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address2CorrespondenceAddressDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PostalDelivery", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always)]
        public Address2PostalDeliveryDetailsType PostalDelivery { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Road", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always)]
        public Address2StreetAddressRoadDetailsType Road { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("LocalityName", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always)]
        public string LocalityName { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("Postcode", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always)]
        public string Postcode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("State", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always)]
        public string State { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("address2PostalDeliveryDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Address2PostalDeliveryDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PostalDeliveryTypeCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PostalDeliveryTypeCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PostalDeliveryNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PostalDeliveryNumber { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("overseasAddress1Type", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class OverseasAddress1Type
    {

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<OverseasAddress1AddressLineDetailsType> _addressLine;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("AddressLine", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<OverseasAddress1AddressLineDetailsType> AddressLine
        {
            get
            {
                return this._addressLine;
            }
            private set
            {
                this._addressLine = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die AddressLine-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the AddressLine collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool AddressLineSpecified
        {
            get
            {
                return (this.AddressLine.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="OverseasAddress1Type" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="OverseasAddress1Type" /> class.</para>
        /// </summary>
        public OverseasAddress1Type()
        {
            this._addressLine = new System.Collections.ObjectModel.Collection<OverseasAddress1AddressLineDetailsType>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PostalCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PostalCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("CountryCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string CountryCode { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("overseasAddress1AddressLineDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class OverseasAddress1AddressLineDetailsType
    {

        /// <summary>
        /// <para xml:lang="de">Ruft den Text ab oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets the text value.</para>
        /// </summary>
        [XmlTextAttribute()]
        public string Value { get; set; }

        /// <summary>
        /// </summary>
        [XmlAttributeAttribute("order", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Order { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("overseasAddress2Type", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class OverseasAddress2Type
    {

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<OverseasAddress2AddressLineDetailsType> _addressLine;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("AddressLine", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<OverseasAddress2AddressLineDetailsType> AddressLine
        {
            get
            {
                return this._addressLine;
            }
            private set
            {
                this._addressLine = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="OverseasAddress2Type" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="OverseasAddress2Type" /> class.</para>
        /// </summary>
        public OverseasAddress2Type()
        {
            this._addressLine = new System.Collections.ObjectModel.Collection<OverseasAddress2AddressLineDetailsType>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PostalCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PostalCode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("CountryCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string CountryCode { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("overseasAddress2AddressLineDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class OverseasAddress2AddressLineDetailsType
    {

        /// <summary>
        /// <para xml:lang="de">Ruft den Text ab oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets the text value.</para>
        /// </summary>
        [XmlTextAttribute()]
        public string Value { get; set; }

        /// <summary>
        /// </summary>
        [XmlAttributeAttribute("order", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Order { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("businessType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class Business
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("LegalEntityName", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always)]
        public string LegalEntityName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("BusinessName", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BusinessName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("BusinessUnit", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string BusinessUnit { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("OrganisationType", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OrganisationType { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<AdministrationStatusDetailsType> _administrationStatus;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("AdministrationStatus", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore, ItemConverterType = typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
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
        [XmlIgnoreAttribute()]
        public bool AdministrationStatusSpecified
        {
            get
            {
                return (this.AdministrationStatus.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="Business" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="Business" /> class.</para>
        /// </summary>
        public Business()
        {
            this._administrationStatus = new System.Collections.ObjectModel.Collection<AdministrationStatusDetailsType>();
            this._identification = new System.Collections.ObjectModel.Collection<IdentificationDetailsType>();
        }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<IdentificationDetailsType> _identification;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Identification", Namespace = "http://api.pexa.net.au/schema/2/")]
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
        [XmlIgnoreAttribute()]
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
    [XmlTypeAttribute("administrationStatusDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class AdministrationStatusDetailsType
    {

        /// <summary>
        /// <para xml:lang="de">Ruft den Text ab oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets the text value.</para>
        /// </summary>
        [XmlTextAttribute()]
        public string Value { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("identificationDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class IdentificationDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Identifier", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Identifier { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("Value", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Value { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("exceptionResponseType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    [XmlRootAttribute("ExceptionResponse", Namespace = "http://api.pexa.net.au/schema/2/")]
    public partial class ExceptionResponseType
    {

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<ExceptionResponseDetailsType> _exceptionList;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ExceptionList", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<ExceptionResponseDetailsType> ExceptionList
        {
            get
            {
                return this._exceptionList;
            }
            private set
            {
                this._exceptionList = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="ExceptionResponseType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ExceptionResponseType" /> class.</para>
        /// </summary>
        public ExceptionResponseType()
        {
            this._exceptionList = new System.Collections.ObjectModel.Collection<ExceptionResponseDetailsType>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("exceptionResponseDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class ExceptionResponseDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Code", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Code { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Message", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Message { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("fullNameType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class FullName
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("NameTitle", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string NameTitle { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<GivenNameOrderType> _givenName;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("GivenName", Namespace = "http://api.pexa.net.au/schema/2/")]
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
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="FullName" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="FullName" /> class.</para>
        /// </summary>
        public FullName()
        {
            this._givenName = new System.Collections.ObjectModel.Collection<GivenNameOrderType>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("FamilyName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string FamilyName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("FamilyNameOrder", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string FamilyNameOrder { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("NameSuffix", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string NameSuffix { get; set; }

        /// <summary>
        /// </summary>
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        [XmlElementAttribute("DateOfBirth", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "date")]
        public System.DateTime DateOfBirthValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die DateOfBirth-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the DateOfBirth property is specified.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        public bool DateOfBirthValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [XmlIgnoreAttribute()]
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
    [XmlTypeAttribute("givenNameOrderType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class GivenNameOrderType
    {

        /// <summary>
        /// <para xml:lang="de">Ruft den Text ab oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets the text value.</para>
        /// </summary>
        [XmlTextAttribute()]
        public string Value { get; set; }

        /// <summary>
        /// </summary>
        [XmlAttributeAttribute("order", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string Order { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("foreignPartyDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class ForeignPartyDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ForeignPartyIndicator", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ForeignPartyIndicator { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ForeignPartyType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ForeignPartyType { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ForeignCountry", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ForeignCountry { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ResidentPartyIndicator", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ResidentPartyIndicator { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<ForeignCountryShareDetailsType> _foreignCountryShare;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ForeignCountryShare", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<ForeignCountryShareDetailsType> ForeignCountryShare
        {
            get
            {
                return this._foreignCountryShare;
            }
            private set
            {
                this._foreignCountryShare = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die ForeignCountryShare-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the ForeignCountryShare collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool ForeignCountryShareSpecified
        {
            get
            {
                return (this.ForeignCountryShare.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="ForeignPartyDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="ForeignPartyDetailsType" /> class.</para>
        /// </summary>
        public ForeignPartyDetailsType()
        {
            this._foreignCountryShare = new System.Collections.ObjectModel.Collection<ForeignCountryShareDetailsType>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("foreignCountryShareDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class ForeignCountryShareDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ForeignShareholderCountry", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ForeignShareholderCountry { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ForeignPercent", Namespace = "http://api.pexa.net.au/schema/2/")]
        public decimal ForeignPercent { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("trustDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class TrustDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("TrustIndicator", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string TrustIndicator { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("TrustName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string TrustName { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("partyCapacityDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class PartyCapacityDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyCapacityType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PartyCapacityType { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyCapacityDetail", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PartyCapacityDetail { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("warningResponseType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class WarningResponseType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Code", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Code { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Message", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Message { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("businessAddressType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class BusinessAddressType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("StreetName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string StreetName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Suburb", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Suburb { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("State", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string State { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Postcode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Postcode { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Country", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Country { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("availableSettlementTimeType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class AvailableSettlementTimeType
    {

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<System.DateTime> _time;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Time", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.Collections.ObjectModel.Collection<System.DateTime> Time
        {
            get
            {
                return this._time;
            }
            private set
            {
                this._time = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="AvailableSettlementTimeType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="AvailableSettlementTimeType" /> class.</para>
        /// </summary>
        public AvailableSettlementTimeType()
        {
            this._time = new System.Collections.ObjectModel.Collection<System.DateTime>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("userDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class UserDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("FirstName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string FirstName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("MiddleName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string MiddleName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LastName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LastName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("JobTitle", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string JobTitle { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Phone", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Phone { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("BusinessAddress", Namespace = "http://api.pexa.net.au/schema/2/")]
        public BusinessAddressType BusinessAddress { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("estateConstraintType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class EstateConstraintType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ConstraintType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ConstraintType { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Identifier", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Identifier { get; set; }

        /// <summary>
        /// </summary>
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        [XmlElementAttribute("RegisteredDate", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime RegisteredDateValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die RegisteredDate-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the RegisteredDate property is specified.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        public bool RegisteredDateValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> RegisteredDate
        {
            get
            {
                if (this.RegisteredDateValueSpecified)
                {
                    return this.RegisteredDateValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.RegisteredDateValue = value.GetValueOrDefault();
                this.RegisteredDateValueSpecified = value.HasValue;
            }
        }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _holders;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("Holders", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("TitleParty", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<string> Holders
        {
            get
            {
                return this._holders;
            }
            private set
            {
                this._holders = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="EstateConstraintType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="EstateConstraintType" /> class.</para>
        /// </summary>
        public EstateConstraintType()
        {
            this._holders = new System.Collections.ObjectModel.Collection<string>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("statutoryChargesEstateConstraintType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class StatutoryChargesEstateConstraintType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ConstraintType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ConstraintType { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Identifier", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Identifier { get; set; }

        /// <summary>
        /// </summary>
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        [XmlElementAttribute("RegisteredDate", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime RegisteredDateValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die RegisteredDate-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the RegisteredDate property is specified.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        public bool RegisteredDateValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> RegisteredDate
        {
            get
            {
                if (this.RegisteredDateValueSpecified)
                {
                    return this.RegisteredDateValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.RegisteredDateValue = value.GetValueOrDefault();
                this.RegisteredDateValueSpecified = value.HasValue;
            }
        }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _holders;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("Holders", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("TitleParty", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<string> Holders
        {
            get
            {
                return this._holders;
            }
            private set
            {
                this._holders = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Holders-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Holders collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool HoldersSpecified
        {
            get
            {
                return (this.Holders.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="StatutoryChargesEstateConstraintType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="StatutoryChargesEstateConstraintType" /> class.</para>
        /// </summary>
        public StatutoryChargesEstateConstraintType()
        {
            this._holders = new System.Collections.ObjectModel.Collection<string>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationDocumentReferenceDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationDocumentReferenceDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("DocumentType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string DocumentType { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("LrDocumentId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LrDocumentId { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<NotificationDocumentStatusDetailsType> _documentStatusDetails;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("DocumentStatusDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<NotificationDocumentStatusDetailsType> DocumentStatusDetails
        {
            get
            {
                return this._documentStatusDetails;
            }
            private set
            {
                this._documentStatusDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="NotificationDocumentReferenceDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="NotificationDocumentReferenceDetailsType" /> class.</para>
        /// </summary>
        public NotificationDocumentReferenceDetailsType()
        {
            this._documentStatusDetails = new System.Collections.ObjectModel.Collection<NotificationDocumentStatusDetailsType>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationDocumentStatusDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationDocumentStatusDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("DocumentStatusTimestamp", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime DocumentStatusTimestamp { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("DocumentStatus", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string DocumentStatus { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceCreationRequestType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    [XmlRootAttribute("WorkspaceCreationRequest", Namespace = "http://api.pexa.net.au/schema/2/")]
    public partial class WorkspaceCreationRequest
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        [XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LandTitleDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public WorkspaceCreationRequestTypeLandTitleDetails LandTitleDetails { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RequestLandTitleData", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string RequestLandTitleData { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Jurisdiction", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Jurisdiction { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public PexaRole Role { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("SubscriberReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string SubscriberReference { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("ProjectId", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ProjectId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("ProjectName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ProjectName { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("WorkgroupId", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string WorkgroupId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("FinancialSettlement", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string FinancialSettlement { get; set; }

        /// <summary>
        /// </summary>
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        [XmlElementAttribute("SettlementDate", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "date")]
        public System.DateTime SettlementDateValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die SettlementDate-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the SettlementDate property is specified.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        public bool SettlementDateValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [XmlIgnoreAttribute()]
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
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        [XmlElementAttribute("SettlementDateAndTime", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime SettlementDateAndTimeValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die SettlementDateAndTime-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the SettlementDateAndTime property is specified.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        public bool SettlementDateAndTimeValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [XmlIgnoreAttribute()]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
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
        [XmlElementAttribute("ParticipantSettlementAcceptanceStatus", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string ParticipantSettlementAcceptanceStatus { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceCreationRequestTypePartyDetailsParty> _partyDetails;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("PartyDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("Party", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<WorkspaceCreationRequestTypePartyDetailsParty> PartyDetails
        {
            get
            {
                return this._partyDetails;
            }
            set
            {
                this._partyDetails = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die PartyDetails-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the PartyDetails collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool PartyDetailsSpecified
        {
            get
            {
                return (this.PartyDetails.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceCreationRequest" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceCreationRequest" /> class.</para>
        /// </summary>
        public WorkspaceCreationRequest()
        {
            this._partyDetails = new System.Collections.ObjectModel.Collection<WorkspaceCreationRequestTypePartyDetailsParty>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceCreationRequestTypeLandTitleDetails", Namespace = "http://api.pexa.net.au/schema/2/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceCreationRequestTypeLandTitleDetails
    {

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceCreationRequestTypeLandTitleDetailsLandTitle> _landTitle;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LandTitle", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always)]
        public System.Collections.ObjectModel.Collection<WorkspaceCreationRequestTypeLandTitleDetailsLandTitle> LandTitle
        {
            get
            {
                return this._landTitle;
            }
            private set
            {
                this._landTitle = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceCreationRequestTypeLandTitleDetails" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceCreationRequestTypeLandTitleDetails" /> class.</para>
        /// </summary>
        public WorkspaceCreationRequestTypeLandTitleDetails()
        {
            this._landTitle = new System.Collections.ObjectModel.Collection<WorkspaceCreationRequestTypeLandTitleDetailsLandTitle>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ParentTitle", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ParentTitle { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceCreationRequestTypeLandTitleDetailsLandTitle", Namespace = "http://api.pexa.net.au/schema/2/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceCreationRequestTypeLandTitleDetailsLandTitle
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LandTitleReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string LandTitleReference { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("UnregisteredLotReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string UnregisteredLotReference { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceCreationResponseType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    [XmlRootAttribute("WorkspaceCreationResponse", Namespace = "http://api.pexa.net.au/schema/2/")]
    public partial class WorkspaceCreationResponse
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string WorkspaceId { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceCreationResponseTypeLandTitleDetailsLandTitle> _landTitleDetails;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("LandTitleDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("LandTitle", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<WorkspaceCreationResponseTypeLandTitleDetailsLandTitle> LandTitleDetails
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
        [XmlIgnoreAttribute()]
        public bool LandTitleDetailsSpecified
        {
            get
            {
                return (this.LandTitleDetails.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceCreationResponse" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceCreationResponse" /> class.</para>
        /// </summary>
        public WorkspaceCreationResponse()
        {
            this._landTitleDetails = new System.Collections.ObjectModel.Collection<WorkspaceCreationResponseTypeLandTitleDetailsLandTitle>();
            this._partyDetails = new System.Collections.ObjectModel.Collection<WorkspaceCreationResponseTypePartyDetailsParty>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("WorkspaceStatus", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string WorkspaceStatus { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceCreationResponseTypePartyDetailsParty> _partyDetails;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("PartyDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("Party", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<WorkspaceCreationResponseTypePartyDetailsParty> PartyDetails
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
        [XmlIgnoreAttribute()]
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
    [XmlTypeAttribute("WorkspaceCreationRequestTypePartyDetailsParty", Namespace = "http://api.pexa.net.au/schema/2/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceCreationRequestTypePartyDetailsParty
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("RepresentingParty", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always)]
        public string RepresentingParty { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyType", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always)]
        public string PartyType { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyRole", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Always)]
        public string PartyRole { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("FullName", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public WCA.PEXA.Client.FullName FullName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Business", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public WCA.PEXA.Client.Business Business { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("CurrentAddress", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public WCA.PEXA.Client.Address2 CurrentAddress { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("FutureAddress", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public WCA.PEXA.Client.Address2 FutureAddress { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyCapacityDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public WCA.PEXA.Client.PartyCapacityDetailsType PartyCapacityDetails { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("TrustDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public WCA.PEXA.Client.TrustDetailsType TrustDetails { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ForeignPartyDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        [JsonProperty(Required = Newtonsoft.Json.Required.Default, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public WCA.PEXA.Client.ForeignPartyDetailsType ForeignPartyDetails { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceCreationResponseTypeLandTitleDetailsLandTitle", Namespace = "http://api.pexa.net.au/schema/2/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceCreationResponseTypeLandTitleDetailsLandTitle
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ObsoleteLandTitleReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ObsoleteLandTitleReference { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("NewLandTitleReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string NewLandTitleReference { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceCreationResponseTypePartyDetailsParty", Namespace = "http://api.pexa.net.au/schema/2/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceCreationResponseTypePartyDetailsParty
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("PartyId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PartyId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PartyType { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyRole", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PartyRole { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("FullName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public WCA.PEXA.Client.FullName FullName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Business", Namespace = "http://api.pexa.net.au/schema/2/")]
        public WCA.PEXA.Client.Business Business { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("CurrentAddress", Namespace = "http://api.pexa.net.au/schema/2/")]
        public WCA.PEXA.Client.Address2 CurrentAddress { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("FutureAddress", Namespace = "http://api.pexa.net.au/schema/2/")]
        public WCA.PEXA.Client.Address2 FutureAddress { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceCreationTitleInformationResponseType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    [XmlRootAttribute("WorkspaceCreationTitleInformationResponse", Namespace = "http://api.pexa.net.au/schema/2/")]
    public partial class WorkspaceCreationTitleInformationResponseType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("WorkspaceCreationResponse", Namespace = "http://api.pexa.net.au/schema/2/")]
        public WCA.PEXA.Client.WorkspaceCreationResponse WorkspaceCreationResponse { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("TitleInformationResponse", Namespace = "http://api.pexa.net.au/schema/2/")]
        public TitleInformationResponseType TitleInformationResponse { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("titleInformationResponseType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    [XmlRootAttribute("TitleInformationResponse", Namespace = "http://api.pexa.net.au/schema/2/")]
    public partial class TitleInformationResponseType
    {

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TitleInformationResponseTypeRegistryTitleDataLandTitle> _registryTitleData;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("RegistryTitleData", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("LandTitle", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<TitleInformationResponseTypeRegistryTitleDataLandTitle> RegistryTitleData
        {
            get
            {
                return this._registryTitleData;
            }
            private set
            {
                this._registryTitleData = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die RegistryTitleData-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the RegistryTitleData collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool RegistryTitleDataSpecified
        {
            get
            {
                return (this.RegistryTitleData.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="TitleInformationResponseType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TitleInformationResponseType" /> class.</para>
        /// </summary>
        public TitleInformationResponseType()
        {
            this._registryTitleData = new System.Collections.ObjectModel.Collection<TitleInformationResponseTypeRegistryTitleDataLandTitle>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("TitleInformationResponseTypeRegistryTitleDataLandTitle", Namespace = "http://api.pexa.net.au/schema/2/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class TitleInformationResponseTypeRegistryTitleDataLandTitle
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LandTitleReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LandTitleReference { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LandDescription", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LandDescription { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Address", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Address { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("EstateType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string EstateType { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancy> _tenancies;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("Tenancies", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("Tenancy", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancy> Tenancies
        {
            get
            {
                return this._tenancies;
            }
            private set
            {
                this._tenancies = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Tenancies-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Tenancies collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool TenanciesSpecified
        {
            get
            {
                return (this.Tenancies.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="TitleInformationResponseTypeRegistryTitleDataLandTitle" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TitleInformationResponseTypeRegistryTitleDataLandTitle" /> class.</para>
        /// </summary>
        public TitleInformationResponseTypeRegistryTitleDataLandTitle()
        {
            this._tenancies = new System.Collections.ObjectModel.Collection<TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancy>();
            this._propertyMortgages = new System.Collections.ObjectModel.Collection<WCA.PEXA.Client.EstateConstraintType>();
            this._propertyCaveats = new System.Collections.ObjectModel.Collection<WCA.PEXA.Client.EstateConstraintType>();
            this._statutoryCharges = new System.Collections.ObjectModel.Collection<WCA.PEXA.Client.StatutoryChargesEstateConstraintType>();
            this._priorityNotices = new System.Collections.ObjectModel.Collection<WCA.PEXA.Client.EstateConstraintType>();
        }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WCA.PEXA.Client.EstateConstraintType> _propertyMortgages;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("PropertyMortgages", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("EstateConstraint", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<WCA.PEXA.Client.EstateConstraintType> PropertyMortgages
        {
            get
            {
                return this._propertyMortgages;
            }
            private set
            {
                this._propertyMortgages = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die PropertyMortgages-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the PropertyMortgages collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool PropertyMortgagesSpecified
        {
            get
            {
                return (this.PropertyMortgages.Count != 0);
            }
        }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WCA.PEXA.Client.EstateConstraintType> _propertyCaveats;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("PropertyCaveats", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("EstateConstraint", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<WCA.PEXA.Client.EstateConstraintType> PropertyCaveats
        {
            get
            {
                return this._propertyCaveats;
            }
            private set
            {
                this._propertyCaveats = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die PropertyCaveats-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the PropertyCaveats collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool PropertyCaveatsSpecified
        {
            get
            {
                return (this.PropertyCaveats.Count != 0);
            }
        }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WCA.PEXA.Client.StatutoryChargesEstateConstraintType> _statutoryCharges;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("StatutoryCharges", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("EstateConstraint", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<WCA.PEXA.Client.StatutoryChargesEstateConstraintType> StatutoryCharges
        {
            get
            {
                return this._statutoryCharges;
            }
            private set
            {
                this._statutoryCharges = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die StatutoryCharges-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the StatutoryCharges collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool StatutoryChargesSpecified
        {
            get
            {
                return (this.StatutoryCharges.Count != 0);
            }
        }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WCA.PEXA.Client.EstateConstraintType> _priorityNotices;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("PriorityNotices", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("EstateConstraint", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<WCA.PEXA.Client.EstateConstraintType> PriorityNotices
        {
            get
            {
                return this._priorityNotices;
            }
            private set
            {
                this._priorityNotices = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die PriorityNotices-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the PriorityNotices collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool PriorityNoticesSpecified
        {
            get
            {
                return (this.PriorityNotices.Count != 0);
            }
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("CertificateOfTitle", Namespace = "http://api.pexa.net.au/schema/2/")]
        public TitleInformationResponseTypeRegistryTitleDataLandTitleCertificateOfTitle CertificateOfTitle { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("TitleActivityCheck", Namespace = "http://api.pexa.net.au/schema/2/")]
        public TitleInformationResponseTypeRegistryTitleDataLandTitleTitleActivityCheck TitleActivityCheck { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("TitleInformationResponseTypeRegistryTitleDataLandTitleCertificateOfTitle", Namespace = "http://api.pexa.net.au/schema/2/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class TitleInformationResponseTypeRegistryTitleDataLandTitleCertificateOfTitle
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Type", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Issued", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Issued { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ControllerName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ControllerName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ControllerIdName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ControllerIdName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ControllerDesignation", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ControllerDesignation { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ReceiptId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ReceiptId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ProductionType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ProductionType { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ProducingPartyName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ProducingPartyName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ProducingPartyCapacity", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ProducingPartyCapacity { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ConsentingPartyName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ConsentingPartyName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ConsentingPartyCapacity", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ConsentingPartyCapacity { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("DocumentType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string DocumentType { get; set; }

        /// <summary>
        /// </summary>
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        [XmlElementAttribute("EditionDate", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime EditionDateValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die EditionDate-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the EditionDate property is specified.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        public bool EditionDateValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> EditionDate
        {
            get
            {
                if (this.EditionDateValueSpecified)
                {
                    return this.EditionDateValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.EditionDateValue = value.GetValueOrDefault();
                this.EditionDateValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("EditionDateString", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string EditionDateString { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("EditionNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string EditionNumber { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Status", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Status { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LrCustomerCode", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LrCustomerCode { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancy", Namespace = "http://api.pexa.net.au/schema/2/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancy
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("TenancyType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string TenancyType { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancyProprietorsProprietor> _proprietors;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("Proprietors", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("Proprietor", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancyProprietorsProprietor> Proprietors
        {
            get
            {
                return this._proprietors;
            }
            private set
            {
                this._proprietors = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Proprietors-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Proprietors collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool ProprietorsSpecified
        {
            get
            {
                return (this.Proprietors.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancy" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancy" /> class.</para>
        /// </summary>
        public TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancy()
        {
            this._proprietors = new System.Collections.ObjectModel.Collection<TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancyProprietorsProprietor>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("TitleInformationResponseTypeRegistryTitleDataLandTitleTitleActivityCheck", Namespace = "http://api.pexa.net.au/schema/2/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class TitleInformationResponseTypeRegistryTitleDataLandTitleTitleActivityCheck
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LastTac", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LastTac { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LastTacIndicator", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LastTacIndicator { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LastPositiveTac", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LastPositiveTac { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancyProprietors" +
        "Proprietor", Namespace = "http://api.pexa.net.au/schema/2/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class TitleInformationResponseTypeRegistryTitleDataLandTitleTenanciesTenancyProprietorsProprietor
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Name", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("NameDetail", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string NameDetail { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Share", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Share { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("retrieveNotificationsResponseType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    [XmlRootAttribute("RetrieveNotificationsResponse", Namespace = "http://api.pexa.net.au/schema/2/")]
    public partial class RetrieveNotificationsResponseType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("NotificationsList", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationsListType NotificationsList { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("NotificationsListType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationsListType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Count", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Count { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("MoreData", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string MoreData { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LastEventTimestamp", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime LastEventTimestamp { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("LastEventId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LastEventId { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<NotificationType> _notification;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Notification", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<NotificationType> Notification
        {
            get
            {
                return this._notification;
            }
            private set
            {
                this._notification = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Notification-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Notification collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool NotificationSpecified
        {
            get
            {
                return (this.Notification.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="NotificationsListType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="NotificationsListType" /> class.</para>
        /// </summary>
        public NotificationsListType()
        {
            this._notification = new System.Collections.ObjectModel.Collection<NotificationType>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SystemGenerated", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SystemGenerated { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("InitiatingSubscriberId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string InitiatingSubscriberId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("InitiatingSubscriberName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string InitiatingSubscriberName { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string WorkspaceId { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<SubscriberReferenceType> _subscriberReference;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SubscriberReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<SubscriberReferenceType> SubscriberReference
        {
            get
            {
                return this._subscriberReference;
            }
            private set
            {
                this._subscriberReference = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die SubscriberReference-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the SubscriberReference collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool SubscriberReferenceSpecified
        {
            get
            {
                return (this.SubscriberReference.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="NotificationType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="NotificationType" /> class.</para>
        /// </summary>
        public NotificationType()
        {
            this._subscriberReference = new System.Collections.ObjectModel.Collection<SubscriberReferenceType>();
        }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("EventId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string EventId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Timestamp", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime Timestamp { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Severity", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Severity { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Category", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Category { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Type", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationTypeKeyValue Type { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Description", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Details", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationDetailsType Details { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("subscriberReferenceType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class SubscriberReferenceType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("WorkspaceRole", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string WorkspaceRole { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Value", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Value { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationTypeKeyValue", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationTypeKeyValue
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Key", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Key { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Value", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Value { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Invitation", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationInvitationDetailsType Invitation { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LandTitle", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationLandTitleDetailsType LandTitle { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Party", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationPartyDetailsType Party { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Document", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationDocumentDetailsType Document { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SettlementSchedule", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationSettlementScheduleDetailsType SettlementSchedule { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("FinancialLineItem", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationFinancialLineItemType FinancialLineItem { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("WorkspaceStatus", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string WorkspaceStatus { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Settlement", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationSettlementDetailsType Settlement { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Lodgement", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationLodgementDetailsType Lodgement { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Participant", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationParticipantDetailsType Participant { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Conversation", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationConversationDetailsType Conversation { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationInvitationDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationInvitationDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Outstanding", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationInvitationOutstandingDetailsType Outstanding { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Response", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationInvitationResponseDetailsType Response { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationInvitationOutstandingDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationInvitationOutstandingDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("InviteId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string InviteId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ForwardedBy", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ForwardedBy { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Timestamp", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime Timestamp { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Jurisdiction", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Jurisdiction { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("WorkspaceRole", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string WorkspaceRole { get; set; }

        /// <summary>
        /// </summary>
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        [XmlElementAttribute("SettlementDate", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime SettlementDateValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die SettlementDate-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the SettlementDate property is specified.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        public bool SettlementDateValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [XmlIgnoreAttribute()]
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
        [XmlElementAttribute("ExpressRefinance", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ExpressRefinance { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<NotificationInvitationOutstandingLandTitleDetailsType> _landTitleDetails;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("LandTitleDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("LandTitle", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<NotificationInvitationOutstandingLandTitleDetailsType> LandTitleDetails
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
        [XmlIgnoreAttribute()]
        public bool LandTitleDetailsSpecified
        {
            get
            {
                return (this.LandTitleDetails.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="NotificationInvitationOutstandingDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="NotificationInvitationOutstandingDetailsType" /> class.</para>
        /// </summary>
        public NotificationInvitationOutstandingDetailsType()
        {
            this._landTitleDetails = new System.Collections.ObjectModel.Collection<NotificationInvitationOutstandingLandTitleDetailsType>();
            this._partyDetails = new System.Collections.ObjectModel.Collection<NotificationInvitationOutstandingPartyDetailsType>();
        }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<NotificationInvitationOutstandingPartyDetailsType> _partyDetails;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("PartyDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("Party", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<NotificationInvitationOutstandingPartyDetailsType> PartyDetails
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
        [XmlIgnoreAttribute()]
        public bool PartyDetailsSpecified
        {
            get
            {
                return (this.PartyDetails.Count != 0);
            }
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("InviterSubscriberDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationInvitationOutstandingInviterSubscriberDetailsType InviterSubscriberDetails { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationInvitationOutstandingInviterSubscriberDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationInvitationOutstandingInviterSubscriberDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SubscriberType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SubscriberType { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SubscriberName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SubscriberName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SubscriberWorkspaceRole", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SubscriberWorkspaceRole { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<NotificationInvitationOutstandingInviterSubscriberDetailsRepresentingPartyType> _representing;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("Representing", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("Party", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<NotificationInvitationOutstandingInviterSubscriberDetailsRepresentingPartyType> Representing
        {
            get
            {
                return this._representing;
            }
            private set
            {
                this._representing = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Representing-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Representing collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool RepresentingSpecified
        {
            get
            {
                return (this.Representing.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="NotificationInvitationOutstandingInviterSubscriberDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="NotificationInvitationOutstandingInviterSubscriberDetailsType" /> class.</para>
        /// </summary>
        public NotificationInvitationOutstandingInviterSubscriberDetailsType()
        {
            this._representing = new System.Collections.ObjectModel.Collection<NotificationInvitationOutstandingInviterSubscriberDetailsRepresentingPartyType>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Notes", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Notes { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationInvitationOutstandingLandTitleDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationInvitationOutstandingLandTitleDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LandTitleReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LandTitleReference { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PropertyDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        public PropertyDetailsType PropertyDetails { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("propertyDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class PropertyDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LandDescription", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LandDescription { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PropertyAddress", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PropertyAddress { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationInvitationOutstandingPartyDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationInvitationOutstandingPartyDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("PartyId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PartyId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyRole", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PartyRole { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public PartyNameDetailsType PartyName { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _associatedLandTitleReference;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("AssociatedLandTitleReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<string> AssociatedLandTitleReference
        {
            get
            {
                return this._associatedLandTitleReference;
            }
            private set
            {
                this._associatedLandTitleReference = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die AssociatedLandTitleReference-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the AssociatedLandTitleReference collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool AssociatedLandTitleReferenceSpecified
        {
            get
            {
                return (this.AssociatedLandTitleReference.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="NotificationInvitationOutstandingPartyDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="NotificationInvitationOutstandingPartyDetailsType" /> class.</para>
        /// </summary>
        public NotificationInvitationOutstandingPartyDetailsType()
        {
            this._associatedLandTitleReference = new System.Collections.ObjectModel.Collection<string>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("partyNameDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class PartyNameDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("FullName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public WCA.PEXA.Client.FullName FullName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("BusinessDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        public WCA.PEXA.Client.Business BusinessDetails { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationInvitationResponseDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationInvitationResponseDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("InviteId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string InviteId { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SubscriberName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SubscriberName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("WorkspaceRole", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string WorkspaceRole { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ResponseReason", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ResponseReason { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("AdditionalText", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string AdditionalText { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationInvitationOutstandingInviterSubscriberDetailsRepresentingPartyType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationInvitationOutstandingInviterSubscriberDetailsRepresentingPartyType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("PartyId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PartyId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyRole", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PartyRole { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public PartyNameDetailsType PartyName { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationLandTitleDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationLandTitleDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LandTitleReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LandTitleReference { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PropertyDetails", Namespace = "http://api.pexa.net.au/schema/2/")]
        public PropertyDetailsType PropertyDetails { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("TitleActivityCheck", Namespace = "http://api.pexa.net.au/schema/2/")]
        public NotificationTitleActivityCheck TitleActivityCheck { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationTitleActivityCheck", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationTitleActivityCheck
    {

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WCA.PEXA.Client.NotificationDocumentReferenceDetailsType> _documentReference;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("DocumentReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<WCA.PEXA.Client.NotificationDocumentReferenceDetailsType> DocumentReference
        {
            get
            {
                return this._documentReference;
            }
            private set
            {
                this._documentReference = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die DocumentReference-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the DocumentReference collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool DocumentReferenceSpecified
        {
            get
            {
                return (this.DocumentReference.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="NotificationTitleActivityCheck" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="NotificationTitleActivityCheck" /> class.</para>
        /// </summary>
        public NotificationTitleActivityCheck()
        {
            this._documentReference = new System.Collections.ObjectModel.Collection<WCA.PEXA.Client.NotificationDocumentReferenceDetailsType>();
            this._administrativeActionReference = new System.Collections.ObjectModel.Collection<NotificationAdministrativeActionReferenceDetailsType>();
        }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<NotificationAdministrativeActionReferenceDetailsType> _administrativeActionReference;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("AdministrativeActionReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<NotificationAdministrativeActionReferenceDetailsType> AdministrativeActionReference
        {
            get
            {
                return this._administrativeActionReference;
            }
            private set
            {
                this._administrativeActionReference = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die AdministrativeActionReference-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the AdministrativeActionReference collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool AdministrativeActionReferenceSpecified
        {
            get
            {
                return (this.AdministrativeActionReference.Count != 0);
            }
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationAdministrativeActionReferenceDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationAdministrativeActionReferenceDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("AdministrativeActionTimestamp", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime AdministrativeActionTimestamp { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("AdministrativeActionType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string AdministrativeActionType { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationPartyDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationPartyDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("PartyId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PartyId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyRole", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string PartyRole { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("PartyName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public PartyNameDetailsType PartyName { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationDocumentDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationDocumentDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("DocumentId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string DocumentId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("DocumentType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string DocumentType { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Status", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Status { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationSettlementScheduleDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationSettlementScheduleDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Status", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Status { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationFinancialLineItemType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationFinancialLineItemType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Type", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Category", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Category { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("OwningSubscriberId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string OwningSubscriberId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("OwningSubscriberName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string OwningSubscriberName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Amount", Namespace = "http://api.pexa.net.au/schema/2/")]
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationSettlementDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationSettlementDetailsType
    {

        /// <summary>
        /// </summary>
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        [XmlElementAttribute("SettlementDate", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime SettlementDateValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die SettlementDate-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the SettlementDate property is specified.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        public bool SettlementDateValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [XmlIgnoreAttribute()]
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
        [XmlElementAttribute("Reason", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Reason { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationLodgementDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationLodgementDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("LodgementCaseId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LodgementCaseId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LodgementCaseStatus", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LodgementCaseStatus { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<NotificationLodgementDocumentDetailsType> _document;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Document", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<NotificationLodgementDocumentDetailsType> Document
        {
            get
            {
                return this._document;
            }
            private set
            {
                this._document = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Document-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Document collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool DocumentSpecified
        {
            get
            {
                return (this.Document.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="NotificationLodgementDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="NotificationLodgementDetailsType" /> class.</para>
        /// </summary>
        public NotificationLodgementDetailsType()
        {
            this._document = new System.Collections.ObjectModel.Collection<NotificationLodgementDocumentDetailsType>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationLodgementDocumentDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationLodgementDocumentDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("DocumentId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string DocumentId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("DocumentType", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string DocumentType { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("DocumentStatus", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string DocumentStatus { get; set; }

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("DealingNumber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string DealingNumber { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _landTitleReference;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LandTitleReference", Namespace = "http://api.pexa.net.au/schema/2/")]
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
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="NotificationLodgementDocumentDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="NotificationLodgementDocumentDetailsType" /> class.</para>
        /// </summary>
        public NotificationLodgementDocumentDetailsType()
        {
            this._landTitleReference = new System.Collections.ObjectModel.Collection<string>();
        }

        /// <summary>
        /// </summary>
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        [XmlElementAttribute("MortgageDate", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "date")]
        public System.DateTime MortgageDateValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die MortgageDate-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the MortgageDate property is specified.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        public bool MortgageDateValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> MortgageDate
        {
            get
            {
                if (this.MortgageDateValueSpecified)
                {
                    return this.MortgageDateValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.MortgageDateValue = value.GetValueOrDefault();
                this.MortgageDateValueSpecified = value.HasValue;
            }
        }

        /// <summary>
        /// </summary>
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        [XmlElementAttribute("Timestamp", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime TimestampValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Timestamp-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the Timestamp property is specified.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        public bool TimestampValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [XmlIgnoreAttribute()]
        public System.Nullable<System.DateTime> Timestamp
        {
            get
            {
                if (this.TimestampValueSpecified)
                {
                    return this.TimestampValue;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                this.TimestampValue = value.GetValueOrDefault();
                this.TimestampValueSpecified = value.HasValue;
            }
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationParticipantDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationParticipantDetailsType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SubscriberName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SubscriberName { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _workspaceRole;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("WorkspaceRole", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<string> WorkspaceRole
        {
            get
            {
                return this._workspaceRole;
            }
            private set
            {
                this._workspaceRole = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="NotificationParticipantDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="NotificationParticipantDetailsType" /> class.</para>
        /// </summary>
        public NotificationParticipantDetailsType()
        {
            this._workspaceRole = new System.Collections.ObjectModel.Collection<string>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("notificationConversationDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class NotificationConversationDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("ConversationId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ConversationId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Timestamp", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime Timestamp { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("subscriberSearchResponseType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    [XmlRootAttribute("SubscriberSearchResponse", Namespace = "http://api.pexa.net.au/schema/2/")]
    public partial class SubscriberSearchResponseType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("OnboardingLink", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string OnboardingLink { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<SubscriberDetailsType> _subscriber;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Subscriber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<SubscriberDetailsType> Subscriber
        {
            get
            {
                return this._subscriber;
            }
            private set
            {
                this._subscriber = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Subscriber-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Subscriber collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool SubscriberSpecified
        {
            get
            {
                return (this.Subscriber.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="SubscriberSearchResponseType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="SubscriberSearchResponseType" /> class.</para>
        /// </summary>
        public SubscriberSearchResponseType()
        {
            this._subscriber = new System.Collections.ObjectModel.Collection<SubscriberDetailsType>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("MoreResults", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string MoreResults { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("subscriberDetailsType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class SubscriberDetailsType
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("SubscriberId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SubscriberId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SubscriberName", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SubscriberName { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Identification", Namespace = "http://api.pexa.net.au/schema/2/")]
        public WCA.PEXA.Client.IdentificationDetailsType Identification { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Email", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Email { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Phone", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Phone { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<string> _operatingIn;

        /// <summary>
        /// </summary>
        [XmlArrayAttribute("OperatingIn", Namespace = "http://api.pexa.net.au/schema/2/")]
        [XmlArrayItemAttribute("Jurisdictions", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<string> OperatingIn
        {
            get
            {
                return this._operatingIn;
            }
            private set
            {
                this._operatingIn = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die OperatingIn-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the OperatingIn collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool OperatingInSpecified
        {
            get
            {
                return (this.OperatingIn.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="SubscriberDetailsType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="SubscriberDetailsType" /> class.</para>
        /// </summary>
        public SubscriberDetailsType()
        {
            this._operatingIn = new System.Collections.ObjectModel.Collection<string>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SubscriberTransactionLimitations", Namespace = "http://api.pexa.net.au/schema/2/")]
        public SubscriberTransactionLimitations SubscriberTransactionLimitations { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("subscriberTransactionLimitations", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    public partial class SubscriberTransactionLimitations
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ReceivingInvitations", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ReceivingInvitations { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LimitationDetail", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LimitationDetail { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("workspaceListRetrievalResponseType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    [XmlRootAttribute("WorkspaceRetrievalResponse", Namespace = "http://api.pexa.net.au/schema/2/")]
    public partial class WorkspaceListRetrievalResponseType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("MoreData", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string MoreData { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceListRetrievalResponseTypeWorkspace> _workspace;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Workspace", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<WorkspaceListRetrievalResponseTypeWorkspace> Workspace
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
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die Workspace-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the Workspace collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool WorkspaceSpecified
        {
            get
            {
                return (this.Workspace.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceListRetrievalResponseType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceListRetrievalResponseType" /> class.</para>
        /// </summary>
        public WorkspaceListRetrievalResponseType()
        {
            this._workspace = new System.Collections.ObjectModel.Collection<WorkspaceListRetrievalResponseTypeWorkspace>();
        }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceListRetrievalResponseTypeWorkspace", Namespace = "http://api.pexa.net.au/schema/2/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceListRetrievalResponseTypeWorkspace
    {

        /// <summary>
        /// <para xml:lang="en">Minimum length: 1.</para>
        /// </summary>
        [MinLengthAttribute(1)]
        [XmlElementAttribute("WorkspaceId", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string WorkspaceId { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("WorkspaceStatus", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string WorkspaceStatus { get; set; }

        /// <summary>
        /// </summary>
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        [XmlElementAttribute("SettlementDateAndTime", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime SettlementDateAndTimeValue { get; set; }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die SettlementDateAndTime-Eigenschaft spezifiziert ist, oder legt diesen fest.</para>
        /// <para xml:lang="en">Gets or sets a value indicating whether the SettlementDateAndTime property is specified.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        [EditorBrowsableAttribute(EditorBrowsableState.Never)]
        public bool SettlementDateAndTimeValueSpecified { get; set; }

        /// <summary>
        /// </summary>
        [XmlIgnoreAttribute()]
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

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WorkspaceListRetrievalResponseTypeWorkspaceSubscriber> _subscriber;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Subscriber", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<WorkspaceListRetrievalResponseTypeWorkspaceSubscriber> Subscriber
        {
            get
            {
                return this._subscriber;
            }
            private set
            {
                this._subscriber = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="WorkspaceListRetrievalResponseTypeWorkspace" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="WorkspaceListRetrievalResponseTypeWorkspace" /> class.</para>
        /// </summary>
        public WorkspaceListRetrievalResponseTypeWorkspace()
        {
            this._subscriber = new System.Collections.ObjectModel.Collection<WorkspaceListRetrievalResponseTypeWorkspaceSubscriber>();
        }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LastActivity", Namespace = "http://api.pexa.net.au/schema/2/", DataType = "dateTime")]
        public System.DateTime LastActivity { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("WorkspaceListRetrievalResponseTypeWorkspaceSubscriber", Namespace = "http://api.pexa.net.au/schema/2/", AnonymousType = true)]
    [DesignerCategoryAttribute("code")]
    public partial class WorkspaceListRetrievalResponseTypeWorkspaceSubscriber
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("SubscriberReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string SubscriberReference { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("Role", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string Role { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("ParticipantSettlementAcceptanceStatus", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string ParticipantSettlementAcceptanceStatus { get; set; }
    }

    /// <summary>
    /// </summary>
    [GeneratedCodeAttribute("XmlSchemaClassGenerator", "2.0.232.0")]
    [SerializableAttribute()]
    [XmlTypeAttribute("titleActivityResponseType", Namespace = "http://api.pexa.net.au/schema/2/")]
    [DesignerCategoryAttribute("code")]
    [XmlRootAttribute("TitleActivityResponse", Namespace = "http://api.pexa.net.au/schema/2/")]
    public partial class TitleActivityResponseType
    {

        /// <summary>
        /// </summary>
        [XmlElementAttribute("TitleActivityStartTime", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string TitleActivityStartTime { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("TitleActivityEndTime", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string TitleActivityEndTime { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("TitleActivityIndicator", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string TitleActivityIndicator { get; set; }

        /// <summary>
        /// </summary>
        [XmlElementAttribute("LastPositiveTitleActivity", Namespace = "http://api.pexa.net.au/schema/2/")]
        public string LastPositiveTitleActivity { get; set; }

        [XmlIgnoreAttribute()]
        private System.Collections.ObjectModel.Collection<WCA.PEXA.Client.NotificationDocumentReferenceDetailsType> _documentReference;

        /// <summary>
        /// </summary>
        [XmlElementAttribute("DocumentReference", Namespace = "http://api.pexa.net.au/schema/2/")]
        public System.Collections.ObjectModel.Collection<WCA.PEXA.Client.NotificationDocumentReferenceDetailsType> DocumentReference
        {
            get
            {
                return this._documentReference;
            }
            private set
            {
                this._documentReference = value;
            }
        }

        /// <summary>
        /// <para xml:lang="de">Ruft einen Wert ab, der angibt, ob die DocumentReference-Collection leer ist.</para>
        /// <para xml:lang="en">Gets a value indicating whether the DocumentReference collection is empty.</para>
        /// </summary>
        [XmlIgnoreAttribute()]
        public bool DocumentReferenceSpecified
        {
            get
            {
                return (this.DocumentReference.Count != 0);
            }
        }

        /// <summary>
        /// <para xml:lang="de">Initialisiert eine neue Instanz der <see cref="TitleActivityResponseType" /> Klasse.</para>
        /// <para xml:lang="en">Initializes a new instance of the <see cref="TitleActivityResponseType" /> class.</para>
        /// </summary>
        public TitleActivityResponseType()
        {
            this._documentReference = new System.Collections.ObjectModel.Collection<WCA.PEXA.Client.NotificationDocumentReferenceDetailsType>();
        }
    }

    [GeneratedCode("NJsonSchema", "9.13.28.0 (Newtonsoft.Json v10.0.0.0)")]
    public enum PexaRole
    {
        [XmlEnum(Name = "Incoming Proprietor")]
        [EnumMember(Value = "Incoming Proprietor")]
        Incoming_Proprietor = 0,

        [XmlEnum(Name = "Incoming Mortgagee")]
        [JsonProperty("Incoming Mortgagee")]
        Incoming_Mortgagee = 1,

        [XmlEnum(Name = "Incoming Caveator")]
        [EnumMember(Value = "Incoming Caveator")]
        Incoming_Caveator = 2,

        [XmlEnum(Name = "Proprietor on Title")]
        [EnumMember(Value = "Proprietor on Title")]
        Proprietor_on_Title = 3,

        [XmlEnum(Name = "Mortgagee on Title")]
        [EnumMember(Value = "Mortgagee on Title")]
        Mortgagee_on_Title = 4,

        [XmlEnum(Name = "Caveator on Title")]
        [EnumMember(Value = "Caveator on Title")]
        Caveator_on_Title = 5,

        [XmlEnum(Name = "Consentor")]
        [EnumMember(Value = "Consentor")]
        Consentor = 6,

        [XmlEnum(Name = "CT Controller")]
        [EnumMember(Value = "CT Controller")]
        CT_Controller = 7,

        [XmlEnum(Name = "Applicant")]
        [EnumMember(Value = "Applicant")]
        Applicant = 8,
    }

    [GeneratedCode("NJsonSchema", "9.13.28.0 (Newtonsoft.Json v10.0.0.0)")]
    public enum WorkspaceCreationResponseWorkspaceStatus
    {
        [XmlEnum(Name = "In Preparation")]
        [EnumMember(Value = "In Preparation")]
        In_Preparation = 0,

    }
}
