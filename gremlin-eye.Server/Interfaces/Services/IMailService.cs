using gremlin_eye.Server.DTOs;

namespace gremlin_eye.Server.Interfaces.Services
{
    public interface IMailService
    {
        bool SendMail(MailData data);
    }
}
