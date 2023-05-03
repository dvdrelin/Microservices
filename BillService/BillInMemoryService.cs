using Model;

namespace BillService;

public sealed class BillInMemoryService : IBillService
{
    private readonly List<Bill> _items = new();

    public void Add(Bill bill)
    {
        var target = _items.FirstOrDefault(x => x.BillId == bill.BillId);
        if (target is not null)
        {
            throw new Exception($"Bill with ID={bill.BillId} already exists!");
        }
        _items.Add(bill);
    }

    public void Delete(Guid billId)
    {
        var target = _items.FirstOrDefault(x => x.BillId == billId);
        if (target is null)
        {
            throw new Exception($"Bill with ID={billId} not found!");
        }

        _items.Remove(target);
    }

    public IEnumerable<Bill> Get() => _items;
}