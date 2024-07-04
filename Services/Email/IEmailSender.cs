namespace Nhom8_DACS.Services.Email
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string content);
    }
}
