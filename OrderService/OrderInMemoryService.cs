using Model;

namespace OrderService;

public sealed class OrderInMemoryService : IOrderService
{
    private readonly List<Order> _items = new();

    public void Add(Order order)
    {
        var target = _items.FirstOrDefault(x => x.OrderId == order.OrderId);
        if (target is not null)
        {
            throw new Exception($"Order with ID={order.OrderId} already exists!");
        }
        _items.Add(order);
    }

    public void Delete(Guid orderId)
    {
        var target = _items.FirstOrDefault(x => x.OrderId == orderId);
        if (target is null)
        {
            throw new Exception($"Order with ID={orderId} not found!");
        }

        _items.Remove(target);
    }

    public IEnumerable<Order> Get() => _items;
}