namespace Model.Events;

public record InventoryRestFailedEvent(Order Order, string Reason);