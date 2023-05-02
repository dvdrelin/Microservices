using MassTransit;
using Model;
using Model.Events;

namespace InventoryService.EventConsumers;

public class OrderCreateEventConsumer : IConsumer<OrderCreateEvent>
{
    private readonly ILogger<OrderCreateEventConsumer> _logger;
    private readonly IInventoryService _inventoryService;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderCreateEventConsumer(ILogger<OrderCreateEventConsumer> logger, IInventoryService inventoryService, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _inventoryService = inventoryService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<OrderCreateEvent> context)
    {
        _logger.LogInformation($"Got an order {context.Message}");

        var stored = _inventoryService.Get().FirstOrDefault(x => x.ProductId == context.Message.Order.ProductId);

        if (stored is not null)
        {
            if (stored.Count > context.Message.Order.ProductCount)
            {
                _inventoryService.Update(new Inventory(context.Message.Order.ProductId, stored.Count - context.Message.Order.ProductCount));
                await _publishEndpoint.Publish(new OrderSuccessfulOnInventoryEvent(context.Message.Order));
                return;
            }
        }
        await _publishEndpoint.Publish(new OrderFailedOnInventoryEvent(context.Message.Order, "There is no enough products."));
    }
}