using FlowersApp.Auth.Shared.Response;
using MediatR;

namespace FlowersApp.Auth.Shared.Interfaces;

public interface IQuery<TResult> : IRequest<RequestResult<TResult>>
{
}

public interface IQueryHandler<TQuery, TResult> : IRequestHandler<TQuery, RequestResult<TResult>>
    where TQuery : IQuery<TResult>
{
}
