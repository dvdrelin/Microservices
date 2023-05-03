using Model;

namespace BillService;

public interface IBillService
{
    void Add(Bill bill);
    void Delete(Guid billId);
    IEnumerable<Bill> Get();
}