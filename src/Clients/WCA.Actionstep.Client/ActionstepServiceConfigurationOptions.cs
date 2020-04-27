using System;

namespace WCA.Actionstep.Client
{
    public class ActionstepServiceConfigurationOptions
    {
        public ActionstepEnvironment ActionstepEnvironment { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public ActionstepServiceConfigurationOptions()
        {
        }

        public ActionstepServiceConfigurationOptions(string clientId, string clientSecret, ActionstepEnvironment actionstepEnvironment = ActionstepEnvironment.Staging)
        {
            ActionstepEnvironment = actionstepEnvironment;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
}