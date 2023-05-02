using Model;

namespace PayService;

public interface IPayService
{
    void Add(Payment payment);
    void Delete(int id);
    IEnumerable<Payment> Get();
}