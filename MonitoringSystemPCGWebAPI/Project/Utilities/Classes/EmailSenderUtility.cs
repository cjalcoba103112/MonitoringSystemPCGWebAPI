using MailKit.Security;
using MimeKit;
using Utilities.Interfaces;

namespace Utilities.Classes
{
    public class EmailSenderUtility : IEmailSenderUtility
    {
        private readonly string _email = "rtc.aurora@coastguard.gov.ph";
        private readonly string _appPassword = "nkyqaxuzwtimtfih";

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            if (string.IsNullOrEmpty(toEmail)) return;

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("RTC Aurora E-Monitoring", _email));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_email, _appPassword);

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

        }
    }
}
