using WCA.Domain.Models.Account;

namespace WCA.Domain.CQRS
{
    public interface IAuthenticatedCommand : ICommand
    {
        WCAUser AuthenticatedUser { get; set; }
    }

    public interface IAuthenticatedCommand<out TResponse> : ICommand<TResponse>
    {
        WCAUser AuthenticatedUser { get; set; }
    }
}
