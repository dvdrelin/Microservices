namespace Model;

public record Payment(Guid PaymentId, Bill Bill, DateTime At, decimal Amount);