using Microsoft.AspNetCore.Mvc;

[ApiController]
    [Route("api/v1/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        // Injecting the industrial Service Layer instead of the raw Repository
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // 1. CREATE A NOTIFICATION
        // POST: api/v1/notification/create
        [HttpPost("create")]
        public IActionResult CreateNotification([FromBody] NotificationDto notificationDto)
        {
            try
            {
                var result = _notificationService.Create(notificationDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    errorType = ErrorTypeCode.DatabaseFailure.ToString(), 
                    message = ex.Message 
                });
            }
        }

        // 2. GET ALL NOTIFICATIONS FOR A SPECIFIC USER
        // GET: api/v1/notification/user/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetNotificationsByUserId(long userId)
        {
            try
            {
                var records = _notificationService.GetByUserId(userId);
                return Ok(records);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    errorType = ErrorTypeCode.UnhandledSystemException.ToString(), 
                    message = ex.Message 
                });
            }
        }

        // 3. DELETE A SINGLE NOTIFICATION
        // DELETE: api/v1/notification/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteNotification(long id)
        {
            try
            {
                if (_notificationService.Delete(id))
                {
                    return Ok(new { message = NotificationMessages.DeleteSuccess });
                }
                return NotFound(new 
                { 
                    errorType = ErrorTypeCode.ResourceNotFound.ToString(), 
                    message = NotificationMessages.NotFound 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // 4. MARK A SINGLE NOTIFICATION AS READ
        // POST: api/v1/notification/{id}/mark-as-read
        [HttpPost("{id}/mark-as-read")]
        public IActionResult MarkAsRead(long id)
        {
            try
            {
                if (_notificationService.MarkAsRead(id)) 
                {
                     // Status 200 with a success message
                    return Ok(new { message = NotificationMessages.MarkAsReadSuccess });
                }
                
                return NotFound(new 
                { 
                    errorType = ErrorTypeCode.ResourceNotFound.ToString(), 
                    message = NotificationMessages.NotFound 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        // 5. MARK ALL NOTIFICATIONS FOR A USER AS READ
        // POST: api/v1/notification/user/{userId}/mark-all-as-read
        [HttpPost("user/{userId}/mark-all-as-read")]
        public IActionResult MarkAllAsRead(long userId)
        {
            try
            {
                if (_notificationService.MarkAllAsRead(userId)) 
                {
                   // Status 200 with a success message
                    return Ok(new { message = NotificationMessages.MarkAsReadSuccess });
                }
                
                return NotFound(new 
                { 
                    errorType = ErrorTypeCode.ResourceNotFound.ToString(), 
                    message = NotificationMessages.BulkNotFound 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }