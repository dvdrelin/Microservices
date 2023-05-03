using Model;

namespace PayService;

public sealed class PaymentInMemoryService : IPaymentService
{
    private readonly List<Payment> _items = new();

    public void Add(Payment payment)
    {
        var target = _items.FirstOrDefault(x => x.PaymentId == payment.PaymentId);
        if (target is not null)
        {
            throw new Exception($"Payment with ID={payment.PaymentId} already exists!");
        }
        _items.Add(payment);
    }

    public void Delete(Guid paymentId)
    {
        var target = _items.FirstOrDefault(x => x.PaymentId == paymentId);
        if (target is null)
        {
            throw new Exception($"Payment with ID={paymentId} not found!");
        }

        _items.Remove(target);
    }

    public IEnumerable<Payment> Get() => _items;
}