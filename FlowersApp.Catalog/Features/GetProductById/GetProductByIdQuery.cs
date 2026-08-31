using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Catalog.Features.GetProductById
{
    public record GetProductByIdQuery(Guid ProductId, Guid? StoreId)
        : IQuery<ProductDetailsResponse>;
}
