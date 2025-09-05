using gremlin_eye.Server.DTOs;

namespace gremlin_eye.Server.Services
{
    public interface IMailService
    {
        bool SendMail(MailData data);
    }
}
