﻿using MassTransit;
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

    [HttpGet] public IEnumerable<Product> Get() => _productService.Get();

    [HttpGet("{id}")] public IActionResult Get(int id)
    {
        var product = _productService.Get().FirstOrDefault(x => x.Id == id);

        return product is null
            ? NotFound($"Product width ID={id} not found!")
            : Ok(product);
    }

    [HttpPost] public async Task Post([FromBody] Product product)
    {
        _productService.Add(product);
        await _publishEndpoint.Publish(new ProductCreatedEvent(product));
    }

    [HttpPut("{id}")] public void Put(int id, [FromBody] Product product) => _productService.Update(id, product);

    [HttpDelete("{id}")] public async Task Delete(int id)
    {
        _productService.Delete(id);
        await _publishEndpoint.Publish(new ProductDeletedEvent(id));
    }
}
