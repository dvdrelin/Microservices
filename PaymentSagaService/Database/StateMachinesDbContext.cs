using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using PaymentSagaService.Database.Configuration;
using PaymentSagaService.Database.Models;
using PaymentSagaService.StateMachines.PaymentStateMachine;

namespace PaymentSagaService.Database;

public class StateMachinesDbContext : SagaDbContext
{
    public DbSet<OrderState>? OrderStates { get; set; }
    public DbSet<CartPosition>? CartPositions { get; set; }

    public StateMachinesDbContext(DbContextOptions options) 
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new CartPositionConfiguration());
        base.OnModelCreating(modelBuilder);
    }

    protected override IEnumerable<ISagaClassMap> Configurations
    {
        get
        {
            yield return new OrderStateMap();
        }
    }
}
