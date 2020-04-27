using WCA.Domain.CQRS;

namespace WCA.Domain.Pexa
{

    public interface IPexaAuthenticatedCommand : IAuthenticatedCommand
    {
        string AccessToken { get; set; }
    }

    public interface IPexaAuthenticatedCommand<out TResponse> : IAuthenticatedCommand<TResponse>
    {
        string AccessToken { get; set; }
    }
}
