using InventoryService;
using InventoryService.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IInventoryService>(new InventoryInMemoryService());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<OrderCreatedEventConsumer>();
    config.AddConsumer<ProductCreatedEventConsumer>();
    config.AddConsumer<ProductDeletedEventConsumer>();
    config.AddConsumer<GetProductCountConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");
        cfg.ReceiveEndpoint("order-create-queue", endpoint => endpoint.ConfigureConsumer<OrderCreatedEventConsumer>(ctx));
        cfg.ReceiveEndpoint("product-create-queue", endpoint => endpoint.ConfigureConsumer<ProductCreatedEventConsumer>(ctx));
        cfg.ReceiveEndpoint("product-delete-queue", endpoint => endpoint.ConfigureConsumer<ProductDeletedEventConsumer>(ctx));
        cfg.ReceiveEndpoint("inventory-rest-queue", endpoint => endpoint.ConfigureConsumer<GetProductCountConsumer>(ctx));
    });
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
