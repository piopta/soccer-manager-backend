using Microsoft.Extensions.Options;
using MimeKit;

namespace WebApi.Services;

public class MailService : IMailService
{
    private readonly MailOptions _mailOptions;
    private readonly IEmailBodyParser _emailBodyParser;
    private readonly ISmtpMailSender _mailSender;

    public MailService(IOptionsMonitor<MailOptions> mailOptions, IEmailBodyParser emailBodyParser,
        ISmtpMailSender mailSender)
    {
        _mailOptions = mailOptions.CurrentValue;
        _emailBodyParser = emailBodyParser;
        _mailSender = mailSender;
    }

    public OneOf<bool, ErrorResultGeneral> SendMail(MailInfo mail)
    {
        try
        {
            using MimeMessage message = new();

            ConstructMessageSendingInfo(message, mail);

            _mailSender.SendMessage(message);

            return true;
        }
        catch (Exception ex)
        {
            return new ErrorResultGeneral(ex.Message);
        }
    }

    private void ConstructMessageSendingInfo(MimeMessage message, MailInfo mail)
    {
        MailboxAddress from = new(_mailOptions.SenderEmail, _mailOptions.SenderEmail);
        MailboxAddress to = new(mail.To, mail.To);

        message.From.Add(from);
        message.To.Add(to);

        message.Subject = mail.Subject;

        message.Body = new TextPart("html") { Text = _emailBodyParser.ReadEmailBody(mail) };
    }
}
