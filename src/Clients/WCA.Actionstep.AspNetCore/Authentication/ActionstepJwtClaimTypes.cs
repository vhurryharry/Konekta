namespace WCA.Actionstep.AspNetCore.Authentication
{
    /// <summary>
    /// Contains additional claims that are supplied with an Actionstep JWT. These are in addition to standard
    /// JWT properties/claims which include:
    ///     "aud": "https://app-test.konekta.com.au",
    ///     "iss": "https://ap-southeast-2.actionstepstaging.com/",
    ///     "sub": "e28813e9d1ea60687fe2a08056e5a6a664a2766c",
    ///     "iat": 1572919093,
    ///     "jti": "c1852af1b77e65468b5382fba892fe6988173b15",
    ///     "nbf": 1572919083,
    ///     "exp": 1572920893
    /// </summary>
    public static class ActionstepJwtClaimTypes
    {
        /// Example: "email": "daniel@workcloud.com.au",
        public const string Email = "email";

        /// Example: "name": "Daniel Smon",
        public const string Name = "name";

        /// Example: "orgkey": "trial181078920",
        public const string Orgkey = "orgkey";

        /// Example: "action_id": 7,
        public const string ActionId = "action_id";

        /// Example: "action_type_id": 7,
        public const string ActionTypeId = "action_type_id";

        /// Example: "action_type_name": "Conveyancing  - Queensland",
        public const string ActionTypeName = "action_type_name";

        /// Example: "timezone": "Australia/Adelaide",
        public const string Timezone = "timezone";

    }
}
