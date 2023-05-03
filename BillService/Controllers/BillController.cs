using Microsoft.AspNetCore.Mvc;
using Model;


namespace BillService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillController(IBillService billService) => _billService = billService;

        [HttpGet]
        public IEnumerable<Bill> Get() => _billService.Get();

        [HttpGet("user/{userId}")]
        public IEnumerable<Bill> GetByUser(int userId) => _billService.Get().Where(x=>x.UserId == userId);

        [HttpGet("{billId}")]
        public Bill Get(Guid billId) => _billService.Get().First(x => x.BillId == billId);

        [HttpPost]
        public void Post([FromBody] int userId)
        {

        }
    }
}
