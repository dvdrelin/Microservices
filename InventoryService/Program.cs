using InventoryService;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IInventoryService>(new InventoryService.InventoryService());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<OrderConsumer>();
    config.AddConsumer<ProductConsumer>();
    config.AddConsumer<ProductDeleteEventConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");
        cfg.ReceiveEndpoint("order-queue", endpoint => endpoint.ConfigureConsumer<OrderConsumer>(ctx));
        cfg.ReceiveEndpoint("product-queue", endpoint => endpoint.ConfigureConsumer<ProductConsumer>(ctx));
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
