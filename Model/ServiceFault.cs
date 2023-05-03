using MassTransit;

namespace Model;

public record ServiceFault(
    Guid FaultId, 
    Guid? FaultedMessageId, 
    DateTime Timestamp, 
    ExceptionInfo[] Exceptions, 
    HostInfo Host, 
    string Message);