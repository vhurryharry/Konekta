using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WCA.Core.Helpers
{
    public static class AddressHelper
    {
        public static readonly Dictionary<string, string> RoadTypeCodes = new Dictionary<string, string>
        {
            {"ACCS", "Access"},
            {"ALLY", "Alley"},
            {"ALWY", "Alleyway"},
            {"AMBL", "Amble"},
            {"ANCG", "Anchorage"},
            {"APP", "Approach"},
            {"ARC", "Arcade"},
            {"ARTL", "Arterial"},
            {"ARTY", "Artery"},
            {"AV", "Avenue"},
            {"BA", "Banan"},
            {"BANK", "Bank"},
            {"BASN", "Basin"},
            {"BAY", "Bay"},
            {"BCH", "Beach"},
            {"BDGE", "Bridge"},
            {"BDWY", "Broadway"},
            {"BEND", "Bend"},
            {"BLK", "Block"},
            {"BOWL", "Bowl"},
            {"BR", "Brace"},
            {"BRAE", "Brae"},
            {"BRK", "Break"},
            {"BROW", "Brow"},
            {"BVD", "Boulevard"},
            {"BVDE", "Boulevarde"},
            {"BWLK", "Boardwalk"},
            {"BYPA", "Bypass"},
            {"BYWY", "Byway"},
            {"CCT", "Circuit"},
            {"CH", "Chase"},
            {"CIR", "Circle"},
            {"CL", "Close"},
            {"CLDE", "Colonnade"},
            {"CLST", "Cluster"},
            {"CLT", "Circlet"},
            {"CMMN", "Common"},
            {"CNR", "Corner"},
            {"CNWY", "Centreway"},
            {"CON", "Concourse"},
            {"COVE", "Cove"},
            {"COWY", "Crossway"},
            {"CPS", "Copse"},
            {"CR", "Crescent"},
            {"CRCS", "Circus"},
            {"CRD", "Crossroad"},
            {"CRSE", "Course"},
            {"CRSG", "Crossing"},
            {"CRSS", "Cross"},
            {"CRST", "Crest"},
            {"CSAC", "Cul-de-sac"},
            {"CSO", "Corso"},
            {"CSWY", "Causeway"},
            {"CT", "Court"},
            {"CTR", "Centre"},
            {"CTT", "Cutting"},
            {"CTYD", "Courtyard"},
            {"CUWY", "Cruiseway"},
            {"CX", "Connection"},
            {"DALE", "Dale"},
            {"DELL", "Dell"},
            {"DENE", "Dene"},
            {"DEVN", "Deviation"},
            {"DIP", "Dip"},
            {"DIV", "Divide"},
            {"DMN", "Domain"},
            {"DOCK", "Dock"},
            {"DR", "Drive"},
            {"DSTR", "Distributor"},
            {"DVWY", "Driveway"},
            {"EDGE", "Edge"},
            {"ELB", "Elbow"},
            {"END", "End"},
            {"ENT", "Entrance"},
            {"ESP", "Esplanade"},
            {"EST", "Estate"},
            {"EXP", "Expressway"},
            {"EXTN", "Extension"},
            {"FAWY", "Fairway"},
            {"FB", "Firebreak"},
            {"FE", "Fireline"},
            {"FITR", "Firetrail"},
            {"FLAT", "Flat"},
            {"FLTS", "Flats"},
            {"FOLW", "Follow"},
            {"FORD", "Ford"},
            {"FORM", "Formation"},
            {"FRNT", "Front"},
            {"FRTG", "Frontage"},
            {"FSHR", "Foreshore"},
            {"FTRK", "Firetrack"},
            {"FTWY", "Footway"},
            {"FWY", "Freeway"},
            {"GAP", "Gap"},
            {"GDN", "Garden"},
            {"GDNS", "Gardens"},
            {"GLDE", "Glade"},
            {"GLEN", "Glen"},
            {"GLY", "Gully"},
            {"GR", "Grove"},
            {"GRA", "Grange"},
            {"GRN", "Green"},
            {"GRND", "Ground"},
            {"GTE", "Gate"},
            {"GTWY", "Gateway"},
            {"HILL", "Hill"},
            {"HOLW", "Hollow"},
            {"HRBR", "Harbour"},
            {"HTH", "Heath"},
            {"HTRD", "Highroad"},
            {"HTS", "Heights"},
            {"HUB", "Hub"},
            {"HWY", "Highway"},
            {"INTG", "Interchange"},
            {"INTN", "Intersection"},
            {"ISLD", "Island"},
            {"JNC", "Junction"},
            {"KEY", "Key"},
            {"KEYS", "Keys"},
            {"LANE", "Lane"},
            {"LDG", "Landing"},
            {"LEES", "Lees"},
            {"LINE", "Line"},
            {"LINK", "Link"},
            {"LKT", "Lookout"},
            {"LNWY", "Laneway"},
            {"LOOP", "Loop"},
            {"LT", "Little"},
            {"LWR", "Lower"},
            {"MALL", "Mall"},
            {"MEW", "Mew"},
            {"MEWS", "Mews"},
            {"MNDR", "Meander"},
            {"MNR", "Manor"},
            {"MT", "Mount"},
            {"MTWY", "Motorway"},
            {"NOOK", "Nook"},
            {"OTLK", "Outlook"},
            {"OTLT", "Outlet"},
            {"PARK", "Park"},
            {"PART", "Part"},
            {"PASS", "Pass"},
            {"PATH", "Path"},
            {"PDE", "Parade"},
            {"PIAZ", "Piazza"},
            {"PKLD", "Parklands"},
            {"PKT", "Pocket"},
            {"PL", "Place"},
            {"PLAT", "Plateau"},
            {"PLZA", "Plaza"},
            {"PNT", "Point"},
            {"PORT", "Port"},
            {"PROM", "Promenade"},
            {"PRST", "Pursuit"},
            {"PSGE", "Passage"},
            {"PWAY", "Pathway"},
            {"PWY", "Parkway"},
            {"QDGL", "Quadrangle"},
            {"QDRT", "Quadrant"},
            {"QUAD", "Quad"},
            {"QY", "Quay"},
            {"QYS", "Quays"},
            {"RAMP", "Ramp"},
            {"RCH", "Reach"},
            {"RD", "Road"},
            {"RDGE", "Ridge"},
            {"RDS", "Roads"},
            {"RDSD", "Roadside"},
            {"RDWY", "Roadway"},
            {"RES", "Reserve"},
            {"REST", "Rest"},
            {"RGWY", "Ridgeway"},
            {"RIDE", "Ride"},
            {"RING", "Ring"},
            {"RISE", "Rise"},
            {"RMBL", "Ramble"},
            {"RND", "Round"},
            {"RNDE", "Ronde"},
            {"RNGE", "Range"},
            {"ROW", "Row"},
            {"ROWY", "Right of Way"},
            {"RSBL", "Rosebowl"},
            {"RSNG", "Rising"},
            {"RTE", "Route"},
            {"RTRN", "Return"},
            {"RTT", "Retreat"},
            {"RTY", "Rotary"},
            {"RUE", "Rue"},
            {"RUN", "Run"},
            {"RVR", "River"},
            {"RVRA", "Riviera"},
            {"RVWY", "Riverway"},
            {"SBWY", "Subway"},
            {"SDNG", "Siding"},
            {"SHUN", "Shunt"},
            {"SHWY", "State Highway"},
            {"SLPE", "Slope"},
            {"SND", "Sound"},
            {"SPUR", "Spur"},
            {"SQ", "Square"},
            {"ST", "Street"},
            {"STPS", "Steps"},
            {"STRA", "Strand"},
            {"STRP", "Strip"},
            {"STRS", "Stairs"},
            {"SVWY", "Service Way"},
            {"TARN", "Tarn"},
            {"TCE", "Terrace"},
            {"THFR", "Thoroughfare"},
            {"TKWY", "Trunkway"},
            {"TLWY", "Tollway"},
            {"TOP", "Top"},
            {"TOR", "Tor"},
            {"TRI", "Triangle"},
            {"TRK", "Track"},
            {"TRL", "Trail"},
            {"TRLR", "Trailer"},
            {"TRWY", "Throughway"},
            {"TURN", "Turn"},
            {"TWIST", "Twist"},
            {"TWRS", "Towers"},
            {"UPAS", "Underpass"},
            {"UPR", "Upper"},
            {"VALE", "Vale"},
            {"VIAD", "Viaduct"},
            {"VIEW", "View"},
            {"VLLS", "Villas"},
            {"VLLY", "Valley"},
            {"VSTA", "Vista"},
            {"VWS", "Views"},
            {"WADE", "Wade"},
            {"WALK", "Walk"},
            {"WAY", "Way"},
            {"WDS", "Woods"},
            {"WHRF", "Wharf"},
            {"WKWY", "Walkway"},
            {"WTRS", "Waters"},
            {"WTWY", "Waterway"},
            {"WYND", "Wynd"},
            {"YARD", "Yard"}
        };
        public static readonly Dictionary<string, string> RoadSuffixCodes = new Dictionary<string, string>
        {
            {"CN", "Central"},
            {"DV", "Deviation"},
            {"E", "East"},
            {"EX", "Extension"},
            {"IN", "Inner"},
            {"LR", "Lower"},
            {"MA", "Mall"},
            {"N", "North"},
            {"NE", "North East"},
            {"NW", "North West"},
            {"OF", "Off"},
            {"ON", "On"},
            {"OU", "Outer"},
            {"S", "South"},
            {"SE", "South East"},
            {"SW", "South West"},
            {"UP", "Upper"},
            {"W", "West"},
        };

        public static readonly Dictionary<string, string> States = new Dictionary<string, string>
        {
            { "ACT", "Australian Capital Territory" },
            { "NSW", "New South Wales" },
            { "NT", "Northern Territory" },
            { "QLD", "Queensland" },
            { "SA", "South Australia" },
            { "TAS", "Tasmania" },
            { "VIC", "Victoria" },
            { "WA", "Western Australia" }
        };

        /// <summary>
        /// Parses the street number.
        /// </summary>
        /// <param name="addressLine1">The address line1.</param>
        /// <param name="addressLine2">The address line2.</param>
        /// <returns>
        /// A Tuple with StreetNumber and StreetName as strings.
        /// StreetNumber is guaranteed to match /^\d+[a-zA-Z]?$/
        /// </returns>
        public static (string StreetNumber, string StreetName)
            ParseStreetNumber(string addressLine1, string addressLine2)
        {
            var streetNumber = string.Empty;
            var streetName = string.Empty;

            if (string.IsNullOrEmpty(addressLine1))
            {
                if (!string.IsNullOrEmpty(addressLine2))
                {
                    streetName = addressLine2;
                }

                return (streetNumber, streetName);
            }

            var streetRegex = new Regex(
                @"((?<strPOBox>((POBox|PO\sBox)\s*\d*)),?\s?)?((?<strUnit>([\w\d\s\,]*)),\s?)?((?<strStreetNumber>(\d)+([a-zA-Z])?)(\s))?(?<strStreet>(([\s\-]*\w\s(st\s))?[\w]*(\s\w*)+))",
                RegexOptions.IgnoreCase | RegexOptions.Multiline);

            var match = streetRegex.Match(addressLine1);
            streetNumber = match.Groups["strStreetNumber"].Value;
            streetName = match.Groups["strStreet"].Value;

            if (!string.IsNullOrEmpty(addressLine2))
            {
                streetName = $"{streetName}, {addressLine2}";
            }

            // A fallback in case StreetNumber doesn't match the required pattern
            if (!Regex.IsMatch(streetNumber, @"^\d+[a-zA-Z]?$"))
            {
                // We shouldn't get here, this is just a fallback
                streetNumber = string.Empty;
                streetName = string.IsNullOrEmpty(addressLine2) ?
                    addressLine1 :
                    $"{addressLine1}, {addressLine2}";
            }

            return (streetNumber, streetName);
        }

        /// <summary>
        /// Parses the road information.
        /// </summary>
        /// <param name="addressLine1">The address line1.</param>
        /// <param name="addressLine2">The address line2.</param>
        /// <returns>
        /// A Tuple with RoadNumber, RoadSuffix, RoadTypeCode and RoadName as strings.
        /// RoadNumber is guaranteed to match /^\d+[a-zA-Z]?$/
        /// </returns>
        public static (string RoadNumber, string RoadSuffixCode, string RoadTypeCode, string RoadName)
            ParseRoadInfo(string addressLine1, string addressLine2)
        {
            var roadSuffixCode = string.Empty;
            var roadTypeCode = string.Empty;
            (var roadNumber, var roadName) = ParseStreetNumber(addressLine1, addressLine2);

            (roadSuffixCode, roadName) = SearchPattern(roadName, RoadSuffixCodes);
            (roadTypeCode, roadName) = SearchPattern(roadName, RoadTypeCodes);

            roadName = roadName.Replace(",", "");
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            roadName = regex.Replace(roadName, " ").Trim();

            return (roadNumber, roadSuffixCode, roadTypeCode, roadName);
        }

        private static (string pattern, string newStr) SearchPattern(string orgStr, Dictionary<string, string> patterns)
        {
            string values = string.Empty;
            string keys = string.Empty;
            Boolean isValueMatched = true;

            foreach (var itr in patterns)
            {
                values += $@"\b{itr.Value}\b|";
                keys += $@"\b{itr.Key}\b|";
            }
            values = values.Remove(values.Length - 1);
            keys = keys.Remove(keys.Length - 1);

            var result = Regex.Match(orgStr, values, RegexOptions.IgnoreCase);

            if (!result.Success)
            {
                result = Regex.Match(orgStr, keys, RegexOptions.IgnoreCase);
                isValueMatched = false;
            }

            if (result.Success)
            {
                orgStr = orgStr.Remove(result.Index, result.Value.Length);

                var keyMatched = result.Value;
                if (isValueMatched)
                {
                    keyMatched = patterns.FirstOrDefault(pattern => string.Equals(pattern.Value, keyMatched, StringComparison.OrdinalIgnoreCase)).Key;
                }

                return (keyMatched, orgStr);
            }

            return (string.Empty, orgStr);
        }

        /// <summary>
        /// Parses the state.
        /// </summary>
        /// <param name="addressLine1">The address line1.</param>
        /// <param name="addressLine2">The address line2.</param>
        /// <returns>
        /// A Tuple with State and AddressLine as strings.
        /// </returns>
        public static (string State, string AddressLine)
            ParseState(string addressLine1, string addressLine2)
        {
            var state = string.Empty;
            var addressLine = string.Empty;

            (state, addressLine) = SearchPattern(addressLine1 + " " + addressLine2, States);

            addressLine = addressLine.Replace(",", "", StringComparison.InvariantCultureIgnoreCase);
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);
            addressLine = regex.Replace(addressLine, " ").Trim();

            return (state, addressLine);
        }
    }
}