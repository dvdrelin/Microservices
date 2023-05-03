using Model;

namespace FaultService;

public sealed class FaultInMemoryService : IFaultService
{
    private readonly List<ServiceFault> _items = new();

    public void Add(ServiceFault serviceFault) => _items.Add(serviceFault);

    public IEnumerable<ServiceFault> Get() => _items;
}