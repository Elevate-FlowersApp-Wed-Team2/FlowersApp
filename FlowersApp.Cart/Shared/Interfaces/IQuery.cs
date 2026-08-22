using FlowersApp.Cart.Shared.Response;
using MediatR;

namespace FlowersApp.Cart.Shared.Interfaces;

public interface IQuery<TResult> : IRequest<RequestResult<TResult>>
{
}

public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, RequestResult<TResult>>
    where TQuery : IQuery<TResult>
{
}
