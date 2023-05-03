using System.Reflection;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PaymentSagaService.Configuration;
using PaymentSagaService.Consumers;
using PaymentSagaService.Database;
using PaymentSagaService.StateMachines.PaymentStateMachine;

Console.Title = Assembly.GetExecutingAssembly().GetName().Name!;
CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            var contextOptions = new DbContextOptionsBuilder()
            .UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection"))
            .Options;

            using (var context = new StateMachinesDbContext(contextOptions))
            {
                context.Database.Migrate();
            }

            var endpointsSection = hostContext.Configuration.GetSection("EndpointsConfiguration");
            var endpointsConfig = endpointsSection.Get<EndpointsConfiguration>();

            services.Configure<EndpointsConfiguration>(endpointsSection);

            var rabbitMqSection = hostContext.Configuration.GetSection("RabbitMqConfiguration");
            var rabbitMqConfig = rabbitMqSection.Get<RabbitMqConfiguration>();

            services.AddDbContext<StateMachinesDbContext>(builder =>
            {
                builder.UseNpgsql(hostContext.Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddMassTransit(x =>
            {
                x.AddSagaRepository<OrderState>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ExistingDbContext<StateMachinesDbContext>();
                        r.ConcurrencyMode = ConcurrencyMode.Optimistic;
                    });

                x.AddSagaStateMachine<OrderStateMachine, OrderState>(typeof(OrderStateMachineDefinition))
                    .Endpoint(e =>
                    {
                        e.Name = endpointsConfig.OrderStateMachineAddress;
                    });

                x.AddDelayedMessageScheduler();
                x.AddConsumer<GetOrderStateConsumer>(typeof(GetOrderStateConsumerDefinition));

                //x.AddRequestClient<GetOrderFromArchive>(new Uri(endpointsConfig.HistoryServiceAddress!));
                //x.AddRequestClient<GetOrderFeedback>(new Uri(endpointsConfig.FeedbackServiceAddress!));
                //x.AddRequestClient<GetCart>(new Uri(endpointsConfig.CartServiceAddress!));


                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.UseDelayedMessageScheduler();
                    cfg.ConfigureEndpoints(context);
                    cfg.Host(rabbitMqConfig.Hostname, rabbitMqConfig.VirtualHost, h =>
                    {
                        h.Username(rabbitMqConfig.Username);
                        h.Password(rabbitMqConfig.Password);
                    });
                });

            }).AddMassTransitHostedService(true);
        });
