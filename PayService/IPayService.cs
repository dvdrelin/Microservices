using Model;

namespace PayService;

public interface IPaymentService
{
    void Add(Payment payment);
    void Delete(Guid paymentId);
    IEnumerable<Payment> Get();
}