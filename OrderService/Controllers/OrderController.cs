using System.Net;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Model;
using Model.Events;
using System.Text.Json;
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
    public Order? Get(int id) => _orderService.Get().FirstOrDefault(x => x.Id == id);

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Order order, CancellationToken cancellationToken)
    {
        try
        {
            var productCount = await _client.GetResponse<GetProductCountResponse>(new GetProductCountRequest(order.ProductId), cancellationToken);
            if (productCount.Message.Rest<order.ProductCount)
            {
                return BadRequest(
                    $"Product rest={productCount.Message.Rest} with product ID={productCount.Message.ProductId} is less than requested({order.ProductCount})");
            }

            _orderService.Add(order);
            await _publishEndpoint.Publish(new OrderCreatedEvent(order), cancellationToken);
            return Ok();
        }
        catch (HttpRequestException httpRequestException) when (httpRequestException.StatusCode == HttpStatusCode.BadGateway)
        {
            return NotFound($"Fetching the product rest with ID={order.ProductId} failed! Remote service is unreachable!");
        }
        catch (HttpRequestException httpRequestException) when (httpRequestException.StatusCode == HttpStatusCode.NotFound)
        {
            return NotFound($"Fetching the product rest failed: {httpRequestException.Message}");
        }
        catch (HttpRequestException httpRequestException) when (httpRequestException.StatusCode is null)
        {
            return BadRequest($"Fetching the product rest failed! \r\n{httpRequestException}");
        }
    }

    [HttpDelete("{id}")]
    public void Delete(int id) => _orderService.Delete(id);
}