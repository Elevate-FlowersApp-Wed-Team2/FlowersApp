using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Features.GetProductCatalog;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Catalog.Features.GetProductCatalog
{
    //public class GetProductCatalogHandler : IQueryHandler<GetProductCatalogQuery, PagedResult<ProductCatalogItemResponse>>
    //{
    //    private readonly Repository<Product> _products;

    //    public GetProductCatalogHandler(Repository<Product> products)
    //    {
    //        _products = products;
    //    }

    //    public async Task<RequestResult<PagedResult<ProductCatalogItemResponse>>> Handle(
    //        GetProductCatalogQuery request, CancellationToken cancellationToken)
    //    {
    //        //var query = _products.Get(p => p.IsActive);

    //        //if (request.StoreId.HasValue)
    //        //    query = query.Where(p => p.StoreId == request.StoreId.Value);

    //        //if (request.CategoryId.HasValue)
    //        //    query = query.Where(p => p.CategoryId == request.CategoryId.Value);

    //        //var totalCount = await query.CountAsync(cancellationToken);

    //        //var items = await query
    //        //    .OrderBy(p => p.Name) //till now ordering by name
    //        //    .Skip((request.PageNumber - 1) * request.PageSize)
    //        //    .Take(request.PageSize)
    //        //    .Select(p => new ProductCatalogItemResponse(
    //        //        p.Id,
    //        //        p.Name,
    //        //        //p.ImageUrl,
    //        //        p.Price,
    //        //        p.DiscountPercentage.HasValue && p.DiscountPercentage > 0
    //        //            ? p.Price - (p.Price * p.DiscountPercentage.Value / 100)
    //        //            : null,
    //        //        p.DiscountPercentage.HasValue && p.DiscountPercentage > 0
    //        //            ? (int)p.DiscountPercentage.Value
    //        //            : null,
    //        //        p.StockQuantity <= 0
    //        //    ))
    //        //    .ToListAsync(cancellationToken);

    //        //var result = new PagedResult<ProductCatalogItemResponse>
    //        //{
    //        //    Items = items,
    //        //    PageNumber = request.PageNumber,
    //        //    PageSize = request.PageSize,
    //        //    TotalCount = totalCount
    //        //};

    //        ////  empty result is still a SUCCESS response, not an error.
    //        //// based on Items.Count == 0, not a failure code.
    //        //return RequestResult<PagedResult<ProductCatalogItemResponse>>.succeeded(
    //        //    result, ResultCode.CatalogRetrieved);
    //    }
    //}
}
