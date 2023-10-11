namespace WebApi.Services
{
    public interface IMailService
    {
        OneOf<bool, ErrorResultGeneral> SendMail(MailInfo mail);
    }
}