using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using Microsoft.EntityFrameworkCore;

namespace FlowersApp.Cart.Features.GetUserCartWithSpecificProduct;

public record GetUserCartWithSpecificProductQuery
(string UserId ,string ProductId) : IQuery<GetUserCartWithSpecificProductResponse>;

public record GetUserCartWithSpecificProductResponse(Guid CartId , ProductData? ProductData );
public record ProductData(string? ProductId, int? Quentity);

public class GetUserCartQueryHandler(Repository<ShoppingCart> repository)
    : IQueryHandler<GetUserCartWithSpecificProductQuery, GetUserCartWithSpecificProductResponse>
{
    private readonly Repository<ShoppingCart> _repository = repository;

    public async Task<RequestResult<GetUserCartWithSpecificProductResponse>> Handle(GetUserCartWithSpecificProductQuery request, CancellationToken cancellationToken)
    {
        var userCart = await _repository.Get(c => c.UserId == request.UserId)
                                          .Select(c => new GetUserCartWithSpecificProductResponse
                                          (
                                               c.Id,
                                               c.Items.Where(i => i.ProductId == request.ProductId)
                                                             .Select(i => new ProductData
                                                             (
                                                                 i.ProductId,
                                                                 i.Quantity
                                                             )).FirstOrDefault()
                                          ))
                                          .FirstOrDefaultAsync(cancellationToken);
        if (userCart is not null)
            return RequestResult<GetUserCartWithSpecificProductResponse>.Failure(ResultCode.NotFoundCartForThisUser);
        return RequestResult<GetUserCartWithSpecificProductResponse>.succeeded(userCart, ResultCode.CartRetrivedSuccesfully);
    }
}
