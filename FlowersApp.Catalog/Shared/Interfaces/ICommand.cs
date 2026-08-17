using FlowersApp.Catalog.Shared.Response;
using MediatR;

namespace FlowersApp.Catalog.Shared.Interfaces;

public interface ICommand<TResult>:IRequest<RequestResult<TResult>>
{}

public interface ICommandHandler<TCommand, TResult> 
    : IRequestHandler<TCommand, RequestResult<TResult>>
    where TCommand : ICommand<TResult>
{
}