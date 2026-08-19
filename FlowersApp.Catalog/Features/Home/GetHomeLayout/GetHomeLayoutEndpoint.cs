
using FlowersApp.Catalog.Shared.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlowersApp.Catalog.Features.Home.GetHomeLayout;

public sealed class GetHomeLayoutEndpoint : IEndpoint
{
    public void Map(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/home").WithTags("Home");

        group.MapGet("/layout", async (
                [FromHeader(Name = "Accept-Language")] string acceptLanguage,
                IStoreContext storeContext,
                ISender sender,
                CancellationToken ct) =>
            {
                // IStoreContext resolves the customer's store (AC8) — currently from the
                // X-Store-Id header (see HeaderStoreContext), swappable for a real resolution
                // strategy (saved address, geo-IP, etc.) without touching this endpoint.
                var result = await sender.Send(new GetHomeLayoutQuery(acceptLanguage, storeContext.StoreId), ct);
                return result.ToHttpResult();
            })
            .AddEndpointFilter<RequireAcceptLanguageFilter>()
            .WithName("GetHomeLayout")
            .WithSummary("Get the backend-driven Home screen layout")
            .WithDescription(
                "Returns an ordered array of Home sections. Each section carries {type, id, " +
                "title?, order, enabled} plus a type-specific payload (banner, category_rail, " +
                "product_rail, occasion_rail). Store-aware via the optional X-Store-Id header.")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
    }
}
