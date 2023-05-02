namespace Model.Events;

public record OrderFailedOnInventoryEvent(Order Order, string Reason);