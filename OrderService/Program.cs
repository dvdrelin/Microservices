
using MassTransit;
using Model.Requests;
using OrderService;
using OrderService.Consumers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IOrderService>(new OrderInMemoryService());
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<InventoryRestApprovedEventConsumer>();
    config.AddConsumer<InventoryRestFailedEventConsumer>();
    config.AddRequestClient<GetProductCountRequest>(new Uri("exchange:inventory-rest-queue"));
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");
        cfg.ReceiveEndpoint("inventory-successful-queue", endpoint => endpoint.ConfigureConsumer<InventoryRestApprovedEventConsumer>(ctx));
        cfg.ReceiveEndpoint("inventory-failed-queue", endpoint => endpoint.ConfigureConsumer<InventoryRestFailedEventConsumer>(ctx));
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