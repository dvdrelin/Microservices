using Model;

namespace BillService;

public interface IBillService
{
    void Add(Bill bill);
    void Delete(int id);
    IEnumerable<Bill> Get();
}