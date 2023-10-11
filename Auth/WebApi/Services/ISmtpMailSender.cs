using MimeKit;

namespace WebApi.Services
{
    public interface ISmtpMailSender
    {
        void Dispose();
        void SendMessage(MimeMessage message);
    }
}