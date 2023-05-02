using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Events;
using System.Net.Http;
using System.Text.Json;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IHttpClientFactory _httpClientFactory;

    public OrderController(
        IOrderService orderService, 
        IPublishEndpoint publishEndpoint,
        IHttpClientFactory httpClientFactory)

    {
        _orderService = orderService;
        _publishEndpoint = publishEndpoint;
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet]
    public IEnumerable<Order> Get() => _orderService.Get();

    [HttpGet("{id}")]
    public Order? Get(int id) => _orderService.Get().FirstOrDefault(x => x.Id == id);

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Order order)
    {
        var productClient = _httpClientFactory.CreateClient("product");

        try
        {
            var productResponse = await productClient.GetStringAsync($"api/Product/{order.ProductId}");
            var product = JsonSerializer.Deserialize<Product>(productResponse);
            if (product is null)
            {
                return NotFound($"Product with ID={order.ProductId} not found!");
            }
            _orderService.Add(order);
            await _publishEndpoint.Publish(new OrderCreateEvent(order));
            return Ok();
        }
        catch (HttpRequestException httpRequestException)
        {
            return BadRequest($"Fetching the product with ID={order.ProductId} failed!\r\n{httpRequestException}");
        }
    }

    [HttpDelete("{id}")]
    public void Delete(int id) => _orderService.Delete(id);
}