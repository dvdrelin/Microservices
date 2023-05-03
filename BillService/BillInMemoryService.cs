using Model;

namespace BillService;

public sealed class BillInMemoryService : IBillService
{
    private readonly List<Bill> _items = new();

    public void Add(Bill bill)
    {
        var target = _items.FirstOrDefault(x => x.Id == bill.Id);
        if (target is not null)
        {
            throw new Exception($"Bill with ID={bill.Id} already exists!");
        }
        _items.Add(bill);
    }

    public void Delete(int id)
    {
        var target = _items.FirstOrDefault(x => x.Id == id);
        if (target is null)
        {
            throw new Exception($"Bill with ID={id} not found!");
        }

        _items.Remove(target);
    }

    public IEnumerable<Bill> Get() => _items;
}