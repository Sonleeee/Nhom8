using System.Net.Mail;
using System.Net;

namespace Nhom8_DACS.Services.Email
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string content)
        {
            var mail = "LamBiengTravel@gmail.com";
            var pw = "rfxu tebq fmwl bece";
            MailMessage mailMessage = new MailMessage();
            mailMessage.IsBodyHtml = true;
            mailMessage.Body = content;
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Timeout = 200000,
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            return client.SendMailAsync(
                new MailMessage(
                    from: mail,
                    to: email,
                    subject,
                    content
                ));
        }
    }
}
