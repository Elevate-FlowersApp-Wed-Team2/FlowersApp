
using FlowersApp.Catalog.Extensions;
using FlowersApp.Catalog.Shared.Response;
using MediatR;
using Microsoft.Extensions.Localization;
using FlowersApp.Catalog.Resources;

using System.Globalization;
using ErrorMessages = FlowersApp.Catalog.Resources.ErrorMessages;

namespace FlowersApp.Catalog.Shared.Behaviours;

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