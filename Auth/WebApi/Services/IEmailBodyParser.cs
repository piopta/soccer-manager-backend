namespace WebApi.Services;

public interface IEmailBodyParser
{
    string ReadEmailBody(MailInfo mailInfo);
}