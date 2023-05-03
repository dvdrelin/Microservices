using MassTransit;
using Model;
using Model.Commands;

namespace FaultService;

public sealed class FaultConsumer:
    IConsumer<Fault<BillOrder>>,
    IConsumer<Fault<PayBill>>
{
    private readonly IFaultService _faultService;
    private readonly ILogger<FaultConsumer> _logger;

    public FaultConsumer(IFaultService faultService, ILogger<FaultConsumer> logger)
    {
        _faultService = faultService;
        _logger = logger;
    }

    public Task Consume(ConsumeContext<Fault<BillOrder>> context)
    {
        _faultService.Add(Map(context.Message));
        _logger.LogError("FAULT CONSUMED: {Message}", context.Message);
        return Task.CompletedTask;
    }

    public Task Consume(ConsumeContext<Fault<PayBill>> context)
    {
        _faultService.Add(Map(context.Message));
        _logger.LogError("FAULT CONSUMED: {Message}", context.Message);
        return Task.CompletedTask;
    }

    private ServiceFault Map<T>(Fault<T> massTransitFault) =>
        new(
            massTransitFault.FaultId,
            massTransitFault.FaultedMessageId,
            massTransitFault.Timestamp,
            massTransitFault.Exceptions,
            massTransitFault.Host,
            massTransitFault.Message?.ToString() ?? string.Empty);
}