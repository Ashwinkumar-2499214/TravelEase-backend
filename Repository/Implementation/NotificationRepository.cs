using System;
using System.Collections.Generic;
using System.Linq;

public class NotificationRepository : INotificationRepository
{
    private readonly AppDbContext _context;

    // 1. Inject your AppDbContext into the constructor here
    public NotificationRepository(AppDbContext context)
    {
        _context = context;
    }

    public long CreateNotification(Notification notification)
    {
        // 2. Save directly to the database context instead of the List
        _context.Notifications.Add(notification);
        _context.SaveChanges();

        // SQL Server automatically handles the auto-incrementing ID 
        // and EF Core populates it right back into this object!
        return notification.NotificationID;
    }

    public Notification? GetNotificationById(long notificationId)
    {
        // Find the record from your real database table
        return _context.Notifications.FirstOrDefault(n => n.NotificationID == notificationId);
    }

    public IEnumerable<Notification> GetNotificationsByUserId(long userId)
    {
        // 4. Fetch the user's notifications directly from the database disk
        return _context.Notifications.Where(n => n.UserID == userId).ToList();
    }

    public bool DeleteNotification(long notificationId)
    {
        var notification = GetNotificationById(notificationId);
        if (notification != null)
        {
            _context.Notifications.Remove(notification);
            _context.SaveChanges(); // Make sure to call SaveChanges to permanently delete
            return true;
        }
        return false;
    }

    public bool MarkAsRead(long notificationId)
    {
        var notification = GetNotificationById(notificationId);
        if (notification != null)
        {
            notification.Status = "Read";
            _context.SaveChanges(); // Persist the update to the database
            return true;
        }
        return false;
    }

    public bool MarkAllAsRead(long userId)
    {
        var userNotifications = _context.Notifications
            .Where(n => n.UserID == userId && n.Status == "Unread")
            .ToList();

        if (userNotifications.Any())
        {
            foreach (var notification in userNotifications)
            {
                notification.Status = "Read";
            }
            _context.SaveChanges(); // Push bulk updates to the database
            return true;
        }
        return false;
    }
}