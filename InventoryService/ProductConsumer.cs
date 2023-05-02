using MassTransit;
using Model;

namespace InventoryService;

public class ProductConsumer : IConsumer<Product>
{
    private const int Minimum = 100;
    private readonly ILogger<ProductConsumer> _logger;
    private readonly IInventoryService _inventoryService;

    public ProductConsumer(ILogger<ProductConsumer> logger, IInventoryService inventoryService)
    {
        _logger = logger;
        _inventoryService = inventoryService;
    }

    public Task Consume(ConsumeContext<Product> context)
    {
        _logger.LogInformation($"Got a new product {context.Message}. Set a default minimum = {Minimum}");

        _inventoryService.Add(new Inventory(context.Message.Id, Minimum));

        return Task.CompletedTask;
    }
}