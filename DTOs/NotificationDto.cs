public class NotificationDto
{
    public long NotificationID { get; set; }
    public long UserID { get; set; }
    public required string Message { get; set; }
    public required string Category { get; set; }
    public string Status { get; set; } = "Unread";
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public List<string>? TargetEmails { get; set; }

}