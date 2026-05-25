public interface INotificationRepository
{
    IEnumerable<Notification> GetNotificationsByUserId(long userId);
    Notification? GetNotificationById(long notificationId); // Notice the '?' here
    long CreateNotification(Notification notification);
    bool DeleteNotification(long notificationId);
    bool MarkAsRead(long notificationId);
    bool MarkAllAsRead(long userId);
}