using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Events;

namespace ProductService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductController(IProductService productService, IPublishEndpoint publishEndpoint)
    {
        _productService = productService;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet] 
    public IEnumerable<Product> Get() => _productService.Get();

    [HttpGet("{id}")]
    public IActionResult Get(Guid productId)
    {
        var product = _productService.Get().FirstOrDefault(x => x.Id == productId);

        return product is null
            ? NotFound($"Product width ID={productId} not found!")
            : Ok(product);
    }

    [HttpPost]
    public async Task Post(string name, decimal price)
    {
        var product = new Product(Guid.NewGuid(), name, price);

        _productService.Add(product);

        await _publishEndpoint.Publish(new ProductCreatedEvent(product));
    }

    [HttpPut("{id}")] 
    public void Put(Guid productId, [FromBody] Product product) => _productService.Update(productId, product);

    [HttpDelete("{id}")]
    public async Task Delete(Guid productId)
    {
        _productService.Delete(productId);
        await _publishEndpoint.Publish(new ProductDeletedEvent(productId));
    }
}
