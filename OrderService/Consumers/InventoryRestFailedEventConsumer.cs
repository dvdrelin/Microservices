using MassTransit;
using Model.Events;

namespace OrderService.Consumers;

public class InventoryRestFailedEventConsumer : IConsumer<InventoryRestFailedEvent>
{
    private readonly IOrderService _orderService;
    private readonly ILogger<InventoryRestFailedEventConsumer> _logger;

    public InventoryRestFailedEventConsumer(
        IOrderService orderService, 
        ILogger<InventoryRestFailedEventConsumer> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<InventoryRestFailedEvent> context)
    {
        _logger.LogInformation($"Order {context.Message.Order.Id} with count={context.Message.Order.ProductCount} of product ID={context.Message.Order.ProductId} has not passed inventory check!");
        _orderService.Delete(context.Message.Order.Id);
        return Task.CompletedTask;
    }
}