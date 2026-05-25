public interface INotificationService
    {
        // 1. Handles creating a notification from incoming DTO values
        NotificationDto Create(NotificationDto dto);

        // 2. Retrieves all notifications for a specific user ID mapped as DTOs
        IEnumerable<NotificationDto> GetByUserId(long userId);

        // 3. Deletes a notification by its unique ID
        bool Delete(long id);

        // 4. Marks a single notification as read
        bool MarkAsRead(long id);

        // 5. Marks all unread notifications for a specific user as read
        bool MarkAllAsRead(long userId);
    }