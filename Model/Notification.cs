namespace Model;

public record Notification(Guid NotificationId, Bill Bill, string Message);