using Newtonsoft.Json;

namespace WCA.Actionstep.Client.Resources.Responses
{
    /// <summary>
    /// Returned along with data ina JSON:API response.
    /// </summary>
    [JsonObject(Title = "Error")]
    public class ActionstepError : IActionstepResponse
    {
        /// <summary>
        /// A unique identifier for this particular occurrence of the problem.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// a URI that MAY yield further details about this particular occurrence of the problem.
        /// </summary>
        public string Href { get; set; }

        /// <summary>
        /// the HTTP status code applicable to this problem, expressed as a string value.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// an application-specific error code, expressed as a string value.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// A short, human-readable summary of the problem. This doesn't change from occurrence to occurrence of the problem, except for purposes of localization.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// A human-readable explanation specific to this occurrence of the problem.
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// Associated resources which can be dereferenced from the request document.
        /// </summary>

        public string Links { get; set; }

        /// <summary>
        /// The relative path to the relevant attribute within the associated resource(s). Will only exist for problems that apply to a single resource or type of resource.
        /// </summary>
        public string Path { get; set; }
    }
}
