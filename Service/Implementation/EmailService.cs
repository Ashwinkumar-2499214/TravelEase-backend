using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    public void SendEmail(string toEmail, string subject, string body)
    {
        try
        {
           
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                
                Credentials = new NetworkCredential("ashwinkumar.s12b@gmail.com", "zyolrlpnksjmfqqj"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                
                From = new MailAddress("ashwinkumar.s12b@gmail.com", "TravelEase Alerts"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);

            
            smtpClient.Send(mailMessage);

            Console.WriteLine($"[SUCCESS] Real email dispatched successfully to {toEmail}!");
        }
        catch (Exception ex)
        {
            
            Console.WriteLine($"[EMAIL ERROR] Delivery Failed: {ex.Message}");
        }
    }
    public void SendBulkEmail(List<string> toEmails, string subject, string body)
    {
        Task.Run(() =>
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("ashwinkumar.s12b@gmail.com", "zyolrlpnksjmfqqj"),
                    EnableSsl = true,
                };

                foreach (var email in toEmails)
                {
                    if (string.IsNullOrEmpty(email)) continue;

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("ashwinkumar.s12b@gmail.com", "TravelEase Bulk Alert"),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(email);
                    smtpClient.Send(mailMessage);

                    
                    Thread.Sleep(1000);
                }
                Console.WriteLine("[SUCCESS] All bulk emails processed in the background!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[BULK ERROR] Failed: {ex.Message}");
            }
        });
    }
}