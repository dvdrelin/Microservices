using Microsoft.AspNetCore.Mvc;
using Model;

namespace FaultService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceFaultController : ControllerBase
    {
        private readonly IFaultService _faultService;

        public ServiceFaultController(IFaultService faultService)
        {
            _faultService = faultService;
        }

        [HttpGet]
        public IEnumerable<ServiceFault> Get()
        {
            return _faultService.Get();
        }

        [HttpGet("{id}")]
        public ServiceFault? Get(Guid id)
        {
            return _faultService.Get().FirstOrDefault(x=>x.FaultId == id);
        }
    }
}
