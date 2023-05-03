using MassTransit;
using Model.Events;

namespace InventoryService.Consumers;

public class ProductDeletedEventConsumer : IConsumer<ProductDeletedEvent>
{
    private const int Minimum = 100;
    private readonly ILogger<ProductDeletedEventConsumer> _logger;
    private readonly IInventoryService _inventoryService;

    public ProductDeletedEventConsumer(ILogger<ProductDeletedEventConsumer> logger, IInventoryService inventoryService)
    {
        _logger = logger;
        _inventoryService = inventoryService;
    }

    public Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        _logger.LogInformation($"Got a new product {context.Message}. Set a default minimum = {Minimum}");

        _inventoryService.Delete(context.Message.ProductId);

        return Task.CompletedTask;
    }
}