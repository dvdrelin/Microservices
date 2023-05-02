using MassTransit;
using Model.Events;

namespace OrderService.EventConsumers;

public class OrderFailedOnInventoryEventConsumer:IConsumer<OrderFailedOnInventoryEvent>
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrderFailedOnInventoryEventConsumer> _logger;

    public OrderFailedOnInventoryEventConsumer(
        IOrderService orderService, 
        ILogger<OrderFailedOnInventoryEventConsumer> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<OrderFailedOnInventoryEvent> context)
    {
        _logger.LogInformation($"Order {context.Message.Order.Id} with count={context.Message.Order.ProductCount} of product ID={context.Message.Order.ProductId} has not passed inventory check!");
        _orderService.Delete(context.Message.Order.Id);
        return Task.CompletedTask;
    }
}