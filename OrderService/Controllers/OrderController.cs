using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Events;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IPublishEndpoint _publishEndpoint;

    public OrderController(
        IOrderService orderService, 
        IPublishEndpoint publishEndpoint)
    {
        _orderService = orderService;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public IEnumerable<Order> Get() => _orderService.Get();

    [HttpGet("{id}")]
    public Order? Get(int id) => _orderService.Get().FirstOrDefault(x => x.Id == id);

    [HttpPost]
    public async Task Post([FromBody] Order order)
    {
        _orderService.Add(order);
        await _publishEndpoint.Publish(new OrderCreateEvent(order));
    }

    [HttpDelete("{id}")]
    public void Delete(int id) => _orderService.Delete(id);
}