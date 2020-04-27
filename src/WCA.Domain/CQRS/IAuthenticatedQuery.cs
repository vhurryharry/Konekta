using WCA.Domain.Models.Account;

namespace WCA.Domain.CQRS
{
    public interface IAuthenticatedQuery : IQuery
    {
        WCAUser AuthenticatedUser { get; set; }
    }

    public interface IAuthenticatedQuery<out TResponse> : IQuery<TResponse>
    {
        WCAUser AuthenticatedUser { get; set; }
    }
}
