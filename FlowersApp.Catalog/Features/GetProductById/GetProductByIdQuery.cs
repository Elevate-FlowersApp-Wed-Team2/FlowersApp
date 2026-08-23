using FlowersApp.Catalog.Domain.Entities;
using FlowersApp.Catalog.Infrastructure.Persistence.Repositories;
using FlowersApp.Catalog.Shared.Interfaces;
using FlowersApp.Catalog.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Catalog.Features.GetProductById;

public record GetProductByIdQuery
(string Id) : IQuery<GetProductByIdResponse>;

public record GetProductByIdResponse(string Id, string Name, decimal Price, bool InStock, int AvailableQty);

public class GetProductByIdQueryHandler(Repository<Product> repository)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResponse>
{
    private readonly Repository<Product> _repository = repository;

    public async Task<RequestResult<GetProductByIdResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.Id, out var id))
            return RequestResult<GetProductByIdResponse>.Failure(ResultCode.ProductNotFound);

        var product = await _repository.Get(p => p.Id == id)
                                 .Select(p => new GetProductByIdResponse
                                 (
                                   p.Id.ToString(),
                                   p.Name,
                                   p.Price,
                                   p.StockQuantity > 0,
                                   p.StockQuantity
                                 )).FirstOrDefaultAsync(cancellationToken);

        if(product == null)
            return RequestResult<GetProductByIdResponse>.Failure(ResultCode.ProductNotFound);

        return RequestResult<GetProductByIdResponse>.succeeded(product,ResultCode.ProductRetrieved);
    }
}
