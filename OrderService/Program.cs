
using MassTransit;
using OrderService;
using OrderService.EventConsumers;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddSingleton<IOrderService>(new OrderService.OrderService());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<OrderSuccessfulOnInventoryEventConsumer>();
    config.AddConsumer<OrderFailedOnInventoryEventConsumer>();
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");
        cfg.ReceiveEndpoint("inventory-successful-queue", endpoint => endpoint.ConfigureConsumer<OrderSuccessfulOnInventoryEventConsumer>(ctx));
        cfg.ReceiveEndpoint("inventory-failed-queue", endpoint => endpoint.ConfigureConsumer<OrderFailedOnInventoryEventConsumer>(ctx));
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