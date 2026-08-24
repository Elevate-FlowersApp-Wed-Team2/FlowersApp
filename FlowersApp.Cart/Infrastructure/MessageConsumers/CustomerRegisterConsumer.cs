using FlowersApp.Cart.Features.InitCart;
using FlowersApp.Shared.MessageContracts;
using MassTransit;
using MediatR;

namespace FlowersApp.Cart.Infrastructure.MessageConsumers;

public class CustomerRegisterConsumer (IMediator mediator ,ILogger<CustomerRegisterConsumer> logger): IConsumer<CustomerRegisterEvent>
{
    private readonly IMediator _mediator = mediator;
    private readonly ILogger<CustomerRegisterConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<CustomerRegisterEvent> context)
    {
        try
        {
            var result = await _mediator.Send(new InitCartCommand(context.Message.UserId));
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed To Consume Event {eventId} : {ex}" , context.CorrelationId, ex);
        }
    }
}
