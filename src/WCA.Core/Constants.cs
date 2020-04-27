namespace WCA.Core
{
    internal static class Constants
    {
        internal static readonly string TestEnvironment = "Test";
        internal static readonly string ActionstepAPIErrorResponse = "Sorry, we couldn't connect to Actionstep.";
        internal static readonly string InfoTrackAPIErrorResponse = "Sorry, we couldn't connect to InfoTrack.";
        internal static readonly int ActionstepMaxFilePartSize = 5 * 1024 * 1024;
        internal static readonly string ActionstepVndJsonMediaType = "application/vnd.api+json";
        internal static readonly string ApplicationJsonMediaType = "application/json";
    }
}
