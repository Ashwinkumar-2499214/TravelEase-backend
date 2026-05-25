public interface IEmailService
    {
        void SendEmail(string toEmail, string subject, string body);
        void SendBulkEmail(List<string> toEmails, string subject, string body);
    }