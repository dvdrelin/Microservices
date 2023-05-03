namespace Model;

public record Bill(int UserId, Guid BillId, Order[] Order);