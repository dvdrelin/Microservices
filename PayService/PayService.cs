using Model;

namespace PayService;

public sealed class PayService : IPayService
{
    private readonly List<Payment> _items = new();

    public void Add(Payment payment)
    {
        var target = _items.FirstOrDefault(x => x.Id == payment.Id);
        if (target is not null)
        {
            throw new Exception($"Payment with ID={payment.Id} already exists!");
        }
        _items.Add(payment);
    }

    public void Delete(int id)
    {
        var target = _items.FirstOrDefault(x => x.Id == id);
        if (target is null)
        {
            throw new Exception($"Payment with ID={id} not found!");
        }

        _items.Remove(target);
    }

    public IEnumerable<Payment> Get() => _items;
}