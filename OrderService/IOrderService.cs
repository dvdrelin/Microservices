using Model;

namespace OrderService;

public interface IOrderService
{
    void Add(Order order);
    void Delete(int id);
    IEnumerable<Order> Get();
}