using MediatR;

namespace WCA.Domain.CQRS
{
    public interface IQuery : IRequest
    {
    }

    public interface IQuery<out TResponse> : IRequest<TResponse>
    {
    }
}
