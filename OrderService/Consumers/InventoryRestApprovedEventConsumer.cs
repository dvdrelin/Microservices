using MassTransit;
using Model.Events;

namespace OrderService.Consumers;

public class InventoryRestApprovedEventConsumer : IConsumer<InventoryRestApprovedEvent>
{
    private readonly ILogger<InventoryRestApprovedEventConsumer> _logger;

    public InventoryRestApprovedEventConsumer(ILogger<InventoryRestApprovedEventConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<InventoryRestApprovedEvent> context)
    {
        _logger.LogInformation($"Order ID={context.Message.Order.OrderId} with count={context.Message.Order.ProductCount} of product ID={context.Message.Order.ProductId} has passed inventory check successfully!");
        return Task.CompletedTask;
    }
}