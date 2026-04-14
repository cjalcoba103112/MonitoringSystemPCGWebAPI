namespace Utilities.Interfaces
{
    public interface IEmailSenderUtility
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
