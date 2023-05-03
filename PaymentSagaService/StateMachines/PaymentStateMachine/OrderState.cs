using MassTransit;
using PaymentSagaService.Database.Models;

namespace PaymentSagaService.StateMachines.PaymentStateMachine;

public class OrderState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }

    public int CurrentState { get; set; }

    public DateTimeOffset? SubmitDate { get; set; }

    public int TotalPrice { get; set; }

    public List<CartPosition> Cart { get; set; } = new();

    public string Manager { get; set; } = string.Empty;

    public bool IsConfirmed { get; set; }

    public DateTimeOffset? ConfirmationDate { get; set; }

    public DateTimeOffset? DeliveryDate { get; set; }

    public byte[] RowVersion { get; set; } = { };

    public Guid? FeedbackReceivingTimeoutToken { get; set; }
}