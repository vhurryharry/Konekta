using NodaTime;

namespace WCA.Web.Areas.API.Integrations
{
    public class RefreshActionstepTokenResponseViewModel
    {
        public Instant RefreshTokenExpiry { get; }

        public RefreshActionstepTokenResponseViewModel(Instant RefreshTokenExpiry)
        {
            this.RefreshTokenExpiry = RefreshTokenExpiry;
        }
    }
}
