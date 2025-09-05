using gremlin_eye.Server.Configurations;
using gremlin_eye.Server.DTOs;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace gremlin_eye.Server.Services
{
    public class MailService : IMailService
    {
        private readonly EmailSettings _mailSettings;

        public MailService(IOptions<EmailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }
        public bool SendMail(MailData data)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(_mailSettings.Name, _mailSettings.EmailId));
                message.To.Add(new MailboxAddress(data.MailToName, data.MailToId));
                message.Subject = data.MailSubject;

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = data.MailBody;
                message.Body = bodyBuilder.ToMessageBody();

                using (var client = new SmtpClient())
                {
                    client.Connect(_mailSettings.Host, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                    client.Send(message);
                    client.Disconnect(true);
                    client.Dispose();
                    return true;
                }
            } catch (Exception ex)
            {
                return false;
            }
        }
    }
}
