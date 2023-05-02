using MassTransit;
using Model.Events;

namespace InventoryService;

public class ProductDeleteEventConsumer : IConsumer<ProductDeleteEvent>
{
    private const int Minimum = 100;
    private readonly ILogger<ProductDeleteEventConsumer> _logger;
    private readonly IInventoryService _inventoryService;

    public ProductDeleteEventConsumer(ILogger<ProductDeleteEventConsumer> logger, IInventoryService inventoryService)
    {
        _logger = logger;
        _inventoryService = inventoryService;
    }

    public Task Consume(ConsumeContext<ProductDeleteEvent> context)
    {
        _logger.LogInformation($"Got a new product {context.Message}. Set a default minimum = {Minimum}");

        _inventoryService.Delete(context.Message.ProductId);

        return Task.CompletedTask;
    }
}