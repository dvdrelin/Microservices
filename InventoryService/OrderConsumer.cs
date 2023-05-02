using MassTransit;
using Model;

namespace InventoryService;

public class OrderConsumer : IConsumer<Order>
{
    private readonly ILogger<OrderConsumer> _logger;
    private readonly IInventoryService _inventoryService;

    public OrderConsumer(ILogger<OrderConsumer> logger, IInventoryService inventoryService)
    {
        _logger = logger;
        _inventoryService = inventoryService;
    }

    public Task Consume(ConsumeContext<Order> context)
    {
        _logger.LogInformation($"Got an order {context.Message}");

        var stored = _inventoryService.Get().FirstOrDefault(x => x.ProductId == context.Message.ProductId);

        if (stored is not null)
        {
            _inventoryService.Update(new Inventory(context.Message.ProductId, stored.Count - context.Message.ProductCount));
        }


        return Task.CompletedTask;
    }
}