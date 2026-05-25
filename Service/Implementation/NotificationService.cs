public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;

        // Constructor injection for architecture loose coupling
        public NotificationService(INotificationRepository notificationRepository, IEmailService emailService)
        {
            _notificationRepository = notificationRepository;
            _emailService = emailService;
        }

        // 1. CREATE NOTIFICATION AND TRIGGER BACKGROUND BULK MAIL
        public NotificationDto Create(NotificationDto dto)
        {
            try
            {
                var notification = new Notification
                {
                    UserID = dto.UserID,
                    Message = dto.Message,
                    Category = dto.Category
                };

                // Commit transaction to database disk storage
                var generatedId = _notificationRepository.CreateNotification(notification);
                
                dto.NotificationID = generatedId;
                dto.Status = notification.Status;
                dto.CreatedDate = notification.CreatedDate;

                // 🔥 ASYNC DISPATCH: If bulk addresses exist, pass them down to the background engine
                if (dto.TargetEmails != null && dto.TargetEmails.Any())
                {
                    string emailSubject = $"TravelEase Update: {dto.Category}";
                    string emailBody = $"<h2>TravelEase Notification Alert</h2><p>{dto.Message}</p>";
                    
                    // Dispatches instantly without pausing or lagging the UI Controller thread
                    _emailService.SendBulkEmail(dto.TargetEmails, emailSubject, emailBody);
                }
                
                return dto;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(NotificationMessages.GeneralError, ex);
            }
        }

        // 2. GET BY USER ID
        public IEnumerable<NotificationDto> GetByUserId(long userId)
        {
            try
            {
                return _notificationRepository.GetNotificationsByUserId(userId)
                    .Select(n => new NotificationDto
                    {
                        NotificationID = n.NotificationID,
                        UserID = n.UserID,
                        Message = n.Message,
                        Category = n.Category,
                        Status = n.Status,
                        CreatedDate = n.CreatedDate,
                        TargetEmails = null // Clean assignment mapping block
                    }).ToList();
            }
            catch (Exception ex)
            {
                throw new ApplicationException(NotificationMessages.GeneralError, ex);
            }
        }

        // 3. DELETE
        public bool Delete(long id)
        {
            try { return _notificationRepository.DeleteNotification(id); }
            catch (Exception ex) { throw new ApplicationException(NotificationMessages.GeneralError, ex); }
        }

        // 4. MARK AS READ
        public bool MarkAsRead(long id)
        {
            try { return _notificationRepository.MarkAsRead(id); }
            catch (Exception ex) { throw new ApplicationException(NotificationMessages.GeneralError, ex); }
        }

        // 5. MARK ALL AS READ
        public bool MarkAllAsRead(long userId)
        {
            try { return _notificationRepository.MarkAllAsRead(userId); }
            catch (Exception ex) { throw new ApplicationException(NotificationMessages.GeneralError, ex); }
        }
    }