using MassTransit;
using Model.Events;

namespace OrderService.EventConsumers;

public class OrderSuccessfulOnInventoryEventConsumer:IConsumer<InventoryRestApprovedEvent>
{
    private readonly ILogger<OrderSuccessfulOnInventoryEventConsumer> _logger;

    public OrderSuccessfulOnInventoryEventConsumer(ILogger<OrderSuccessfulOnInventoryEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<InventoryRestApprovedEvent> context)
    {
        _logger.LogInformation($"Order ID={context.Message.Order.Id} with count={context.Message.Order.ProductCount} of product ID={context.Message.Order.ProductId} has passed inventory check successfully!");
        return Task.CompletedTask;
    }
}