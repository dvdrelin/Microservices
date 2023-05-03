using Microsoft.AspNetCore.Mvc;
using Model;

namespace InventoryService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly IInventoryService _inventoryService;

    public InventoryController(IInventoryService inventoryService) => _inventoryService = inventoryService;

    [HttpGet]
    public IEnumerable<Inventory> Get() => _inventoryService.Get();

    [HttpGet("{id}")]
    public Inventory? Get(int id) => _inventoryService.Get().FirstOrDefault(x => x.ProductId == id);

    [HttpPost]
    public void Post([FromBody] Inventory inventory) => _inventoryService.Add(inventory);

    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Inventory inventory) => _inventoryService.Update(inventory);
}