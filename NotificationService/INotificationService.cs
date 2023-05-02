using Model;

namespace NotificationService;

public interface INotificationService
{
    void Add(Notification bill);
    void Delete(int id);
    IEnumerable<Notification> Get();
}