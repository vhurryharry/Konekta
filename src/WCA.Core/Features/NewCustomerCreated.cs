namespace WCA.Core.Features
{
    public class NewCustomerCreated
    {
        public string ActionstepInstallLink { get; }

        public NewCustomerCreated(string actionstepInstallLink)
        {
            ActionstepInstallLink = actionstepInstallLink;
        }
    }
}