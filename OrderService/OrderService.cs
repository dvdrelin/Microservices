using Model;

namespace OrderService;

public class OrderService : IOrderService
{
    private readonly List<Order> _items = new();

    public void Add(Order order)
    {
        var target = _items.FirstOrDefault(x => x.Id == order.Id);
        if (target is not null)
        {
            throw new Exception($"Order with ID={order.Id} already exists!");
        }
        _items.Add(order);
    }

    public void Delete(int id)
    {
        var target = _items.FirstOrDefault(x => x.Id == id);
        if (target is null)
        {
            throw new Exception($"Order with ID={id} not found!");
        }

        _items.Remove(target);
    }

    public IEnumerable<Order> Get() => _items;
}