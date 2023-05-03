using Model;

namespace InventoryService;

public interface IInventoryService
{
    void Add(Inventory inventory);
    void Update(Inventory inventory);
    void Delete(Guid productId);
    IEnumerable<Inventory> Get();
}