using Model;

namespace NotificationService;

public sealed class NotificationService : INotificationService
{
    private readonly List<Notification> _items = new();


    public void Add(Notification notification)
    {
        var target = _items.FirstOrDefault(x => x.Id == notification.Id);
        if (target is not null)
        {
            throw new Exception($"Notification with ID={notification.Id} already exists!");
        }
        _items.Add(notification);
    }

    public void Delete(int id)
    {
        var target = _items.FirstOrDefault(x => x.Id == id);
        if (target is null)
        {
            throw new Exception($"Notification with ID={id} not found!");
        }

        _items.Remove(target);
    }

    public IEnumerable<Notification> Get() => _items;
}