using Model;

namespace OrderService;

public interface IOrderService
{
    void Add(Order order);
    void Delete(Guid orderId);
    IEnumerable<Order> Get();
}