using MassTransit;
using Model;
using Model.Events;

namespace InventoryService.EventConsumers;

public class ProductCreateEventConsumer : IConsumer<ProductCreateEvent>
{
    private const int Minimum = 100;
    private readonly ILogger<ProductCreateEventConsumer> _logger;
    private readonly IInventoryService _inventoryService;

    public ProductCreateEventConsumer(ILogger<ProductCreateEventConsumer> logger, IInventoryService inventoryService)
    {
        _logger = logger;
        _inventoryService = inventoryService;
    }

    public Task Consume(ConsumeContext<ProductCreateEvent> context)
    {
        _logger.LogInformation($"Got a new product {context.Message}. Set a default minimum = {Minimum}");

        _inventoryService.Add(new Inventory(context.Message.Product.Id, Minimum));

        return Task.CompletedTask;
    }
}