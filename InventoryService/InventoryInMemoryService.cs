using Model;

namespace InventoryService;

public sealed class InventoryInMemoryService : IInventoryService
{
    private readonly List<Inventory> _items = new();

    public void Add(Inventory inventory)
    {
        var target = _items.FirstOrDefault(x => x.ProductId == inventory.ProductId);
        if (target is not null)
        {
            throw new Exception($"Inventory with ProductID={inventory.ProductId} already exists!");
        }
        _items.Add(inventory);
    }

    public void Update(Inventory inventory)
    {
        var target = _items.FirstOrDefault(x => x.ProductId == inventory.ProductId);
        if (target is null)
        {
            throw new Exception($"Inventory with ProductID={inventory.ProductId} not found!");
        }

        _items.Remove(target);
        _items.Add(inventory);
    }

    public void Delete(Guid productId)
    {
        var target = _items.FirstOrDefault(x => x.ProductId == productId);
        if (target is null)
        {
            throw new Exception($"Inventory with ProductID={productId} not found!");
        }

        _items.Remove(target);
    }

    public IEnumerable<Inventory> Get() => _items;
}