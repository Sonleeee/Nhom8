namespace Nhom8.Services.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string content);
    }
}
