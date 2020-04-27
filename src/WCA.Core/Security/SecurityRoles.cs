namespace WCA.Core.Security
{
    public enum SecurityRoles
    {
        /// <summary>
        /// Global admin type things. Like logging in as another user, and setting advanced server settings.
        /// </summary>
        GlobalAdministrator = 0,

        /// <summary>
        /// For feature flagging alpha functionality. Likely only devs will be in this role.
        /// </summary>
        AlphaTester,

        /// <summary>
        /// For feature flagging beta functionality. Likely only WCA staff will be in here.
        /// </summary>
        BetaTester,

        /// <summary>
        /// For cypress test, this user can have password to login without Actionstep OpenID transaction.
        /// </summary>
        AllowedToHavePassword,

        /// <summary>
        /// Granted access to the Admin site
        /// </summary>
        AllowAdminSite,
    }
}