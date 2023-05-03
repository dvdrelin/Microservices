using Model;

namespace FaultService;

public interface IFaultService
{
    void Add(ServiceFault serviceFault);
    IEnumerable<ServiceFault> Get();
}