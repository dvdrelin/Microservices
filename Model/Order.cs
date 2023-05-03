namespace Model;

public record Order(int UserId, Guid OrderId, Guid ProductId, int ProductCount);
