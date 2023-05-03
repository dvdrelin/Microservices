using InventoryService;
using InventoryService.EventConsumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IInventoryService>(new InventoryService.InventoryInMemoryService());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<OrderCreateEventConsumer>();
    config.AddConsumer<ProductCreateEventConsumer>();
    config.AddConsumer<ProductDeleteEventConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");
        cfg.ReceiveEndpoint("order-create-queue", endpoint => endpoint.ConfigureConsumer<OrderCreateEventConsumer>(ctx));
        cfg.ReceiveEndpoint("product-create-queue", endpoint => endpoint.ConfigureConsumer<ProductCreateEventConsumer>(ctx));
        cfg.ReceiveEndpoint("product-delete-queue", endpoint => endpoint.ConfigureConsumer<ProductDeleteEventConsumer>(ctx));
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
