using FlowersApp.Cart.Domain.Entities;
using FlowersApp.Cart.Infrastructure.Persistence.Repositories;
using FlowersApp.Cart.Shared.Interfaces;
using FlowersApp.Cart.Shared.Response;

namespace FlowersApp.Cart.Features.InitCart;

public record InitCartCommand
(string UserId) : ICommand<InitCartResponse>;

public record InitCartResponse(bool Initiated ,Guid ShoppingCartId);

public class InitCartCommandHandler(Repository<ShoppingCart> repository ,ILogger<InitCartCommandHandler> logger)
    : ICommandHandler<InitCartCommand, InitCartResponse>
{
    private readonly Repository<ShoppingCart> _repository = repository;
    private readonly ILogger<InitCartCommandHandler> _logger = logger;

    public async Task<RequestResult<InitCartResponse>> Handle(InitCartCommand request, CancellationToken cancellationToken)
    {
        var cart = new ShoppingCart
        {
            Id = Guid.NewGuid(),
            UserId = request.UserId,
        };

        _repository.Add(cart);
        try
        {
            var affectedRows = await _repository.SaveChangeAsync(cancellationToken);
            if (affectedRows != 1)
                return RequestResult<InitCartResponse>.Failure(ResultCode.FailedToInitiateCart);
            return RequestResult<InitCartResponse>.succeeded(new InitCartResponse(true, cart.Id), ResultCode.CartInitiatedSuccessfully);
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed To Initate Cart For User : {UserId} due to :{ex}" , request.UserId, ex);
            return RequestResult<InitCartResponse>.Failure(ResultCode.FailedToInitiateCart);
        }
    }
}
