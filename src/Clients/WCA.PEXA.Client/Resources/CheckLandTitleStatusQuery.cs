using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Xml.Serialization;

namespace WCA.PEXA.Client.Resources
{
    public class LandTitleReferenceAndJurisdiction
    {
        public LandTitleReferenceAndJurisdiction(string landTitleReference, string jurisdiction)
        {
            LandTitleReference = landTitleReference;
            Jurisdiction = jurisdiction;
        }

        public string LandTitleReference { get; set; }

        public string Jurisdiction { get; set; }
    }

    public class CheckLandTitleStatusQuery : PEXARequestBase
    {
        public CheckLandTitleStatusQuery(LandTitleReferenceAndJurisdiction landTitleReferenceAndJurisdiction, string bearerToken)
        {
            LandTitleReferenceAndJurisdiction = landTitleReferenceAndJurisdiction;
            BearerToken = bearerToken;
        }

        public LandTitleReferenceAndJurisdiction LandTitleReferenceAndJurisdiction { get; set; }

        public override string Path
        {
            get
            {
                if (LandTitleReferenceAndJurisdiction != null)
                    return $"/v1/landRegistry/titleStatus?landTitleReference={LandTitleReferenceAndJurisdiction.LandTitleReference}&jurisdiction={LandTitleReferenceAndJurisdiction.Jurisdiction}";

                return "/v1/landRegistry/titleStatus";
            }
        }

        public override HttpMethod HttpMethod => HttpMethod.Get;

        public override HttpContent Content => null;

        public override int Version => 1;
    }
}
