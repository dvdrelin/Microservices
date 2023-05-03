using Model;

namespace NotificationService;

public interface INotificationService
{
    void Add(Notification bill);
    void Delete(Guid notificationId);
    IEnumerable<Notification> Get();
}