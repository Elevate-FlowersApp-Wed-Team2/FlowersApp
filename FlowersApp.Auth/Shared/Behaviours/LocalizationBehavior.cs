using FlowersApp.Auth.Extensions;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.Extensions.Localization;

namespace FlowersApp.Auth.Shared.Behaviours;

public class LocalizationBehavior<TRequest, TResponse>(IStringLocalizer<ErrorMessages> localizer)
    : IPipelineBehavior<TRequest, TResponse>
    where TResponse : RequestResult<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        var response = await next();
        response.Message = response.Code.Localize(localizer);
        return response;
    }
}