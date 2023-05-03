using MassTransit;
using Model;
using Model.Events;

namespace InventoryService.Consumers;

public class ProductCreatedEventConsumer : IConsumer<ProductCreatedEvent>
{
    private const int Minimum = 100;
    private readonly ILogger<ProductCreatedEventConsumer> _logger;
    private readonly IInventoryService _inventoryService;

    public ProductCreatedEventConsumer(ILogger<ProductCreatedEventConsumer> logger, IInventoryService inventoryService)
    {
        _logger = logger;
        _inventoryService = inventoryService;
    }

    public Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        _logger.LogInformation($"Got a new product {context.Message}. Set a default minimum = {Minimum}");

        _inventoryService.Add(new Inventory(context.Message.Product.Id, Minimum));

        return Task.CompletedTask;
    }
}