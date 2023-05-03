using MassTransit;
using Model.Requests;

namespace InventoryService.Consumers;

public class GetProductCountConsumer : IConsumer<GetProductCountRequest>
{
    private readonly IInventoryService _inventoryService;

    public GetProductCountConsumer(IInventoryService inventoryService)
    {
        _inventoryService = inventoryService;
    }
    public async Task Consume(ConsumeContext<GetProductCountRequest> context)
    {
        var inventory = _inventoryService.Get().FirstOrDefault(x => x.ProductId == context.Message.ProductId)
            ?? throw new InvalidOperationException("Product rest not found!");

        await context.RespondAsync<GetProductCountResponse>(new
        {
            context.Message.ProductId,
            Rest = inventory.Count
        });
    }
}