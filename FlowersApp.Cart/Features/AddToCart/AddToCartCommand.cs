using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Features.GetUserCartWithSpecificProduct;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;
using MediatR;
namespace FlowersApp.Cart.Features.AddToCart;

public record AddToCartCommand
(string UserId, string ProductId, string ProductName, int? Quentity)
    : ICommand<AddToCartResponse>;

public record AddToCartResponse(bool Added);

public class AddToCartCommandHandler(Repository<ShoppingCart> repository 
    , ILogger<AddToCartCommandHandler> logger ,IMediator mediator)
    : ICommandHandler<AddToCartCommand, AddToCartResponse>
{
    private readonly Repository<ShoppingCart> _repository = repository;
    private readonly ILogger<AddToCartCommandHandler> _logger = logger;
    private readonly IMediator _mediator = mediator;

    public async Task<RequestResult<AddToCartResponse>> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _mediator.Send(new GetUserCartWithSpecificProductQuery(request.UserId, request.ProductId), cancellationToken);
        if (!cart.Success)
            return RequestResult<AddToCartResponse>.Failure(cart.Code);
        var product =
    }
}

