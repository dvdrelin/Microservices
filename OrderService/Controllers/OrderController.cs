using System.Net;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Events;
using Model.Requests;

namespace OrderService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OrderController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRequestClient<GetProductCountRequest> _client;

    public OrderController(
        IOrderService orderService,
        IPublishEndpoint publishEndpoint,
        IRequestClient<GetProductCountRequest> client)

    {
        _orderService = orderService;
        _publishEndpoint = publishEndpoint;
        _client = client;
    }

    [HttpGet]
    public IEnumerable<Order> Get() => _orderService.Get();

    [HttpGet("{id}")]
    public Order? Get(Guid orderId) => _orderService.Get().FirstOrDefault(x => x.OrderId == orderId);

    [HttpPost]
    public async Task<IActionResult> Post(
        int userId,
        Guid productId,
        int count,
        CancellationToken cancellationToken)
    {
        var order = new Order(userId, Guid.NewGuid(), productId, count);

        var productCount = await _client.GetResponse<GetProductCountResponse>(new GetProductCountRequest(order.ProductId), cancellationToken);

        if (productCount.Message.Rest < order.ProductCount)
        {
            return BadRequest(
                $"Product rest={productCount.Message.Rest} with product ID={productCount.Message.ProductId} is less than requested({order.ProductCount})");
        }

        _orderService.Add(order);
        await _publishEndpoint.Publish(new OrderCreatedEvent(order), cancellationToken);

        return Ok();
    }

    [HttpDelete("{orderId}")]
    public void Delete(Guid orderId) => _orderService.Delete(orderId);
}