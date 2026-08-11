
using FlowersApp.Auth.Extensions;
using FlowersApp.Auth.Shared.Response;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace FlowersApp.Auth.Shared.Behaviours;

public class LocalizationBehavior<TRequest, TResult>(IStringLocalizer<ErrorMessages> localizer, 
    ILogger<LocalizationBehavior<TRequest, TResult>> logger)
    : IPipelineBehavior<TRequest, RequestResult<TResult>>
    where TRequest : IRequest<RequestResult<TResult>>
{
    public async Task<RequestResult<TResult>> Handle(
        TRequest request,
        RequestHandlerDelegate<RequestResult<TResult>> next,
        CancellationToken ct)
    {
        var response = await next();
        logger.LogInformation("CurrentCulture: {culture}, localized: {val}", CultureInfo.CurrentUICulture, localizer["YourKey"]);
        var test = localizer["test"];
        response.Message = response.Code.Localize(localizer);
        return response;
    }

}