using MassTransit;
using Model;
using Model.Events;

namespace InventoryService.Consumers;

public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventConsumer> _logger;
    private readonly IInventoryService _inventoryService;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderCreatedEventConsumer(ILogger<OrderCreatedEventConsumer> logger, IInventoryService inventoryService, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _inventoryService = inventoryService;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        _logger.LogInformation($"Got an order {context.Message}");

        var stored = _inventoryService.Get().FirstOrDefault(x => x.ProductId == context.Message.Order.ProductId);

        if (stored is not null)
        {
            if (stored.Count > context.Message.Order.ProductCount)
            {
                _inventoryService.Update(new Inventory(context.Message.Order.ProductId, stored.Count - context.Message.Order.ProductCount));
                await _publishEndpoint.Publish(new InventoryRestApprovedEvent(context.Message.Order));
                return;
            }
        }
        await _publishEndpoint.Publish(new InventoryRestFailedEvent(context.Message.Order, "There is no enough products."));
    }
}