using Model;

namespace NotificationService;

public sealed class NotificationInMemoryService : INotificationService
{
    private readonly List<Notification> _items = new();

    public void Add(Notification notification)
    {
        var target = _items.FirstOrDefault(x => x.NotificationId == notification.NotificationId);
        if (target is not null)
        {
            throw new Exception($"Notification with ID={notification.NotificationId} already exists!");
        }
        _items.Add(notification);
    }

    public void Delete(Guid notificationId)
    {
        var target = _items.FirstOrDefault(x => x.NotificationId == notificationId) ??
                     throw new Exception($"Notification with ID={notificationId} not found!");

        _items.Remove(target);
    }

    public IEnumerable<Notification> Get() => _items;
}