using WCA.Domain.CQRS;

namespace WCA.Domain.Pexa
{
    public interface IPexaAuthenticatedQuery : IAuthenticatedQuery
    {
        string AccessToken { get; set; }
    }

    public interface IPexaAuthenticatedQuery<out TResponse> : IAuthenticatedQuery<TResponse>
    {
        string AccessToken { get; set; }
    }
}
