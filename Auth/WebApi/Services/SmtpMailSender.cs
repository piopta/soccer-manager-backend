using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace WebApi.Services;

//IDisposable implementation here: https://dotnettips.wordpress.com/2021/10/29/everything-that-every-net-developer-needs-to-know-about-disposable-types-properly-implementing-the-idisposable-interface/#:~:text=To%20prevent%20virtual%20memory%20leaks%20in%20types%20that,to%20handle%20resource%20cleanup%20and%20memory%20management%20effectively.
public class SmtpMailSender : IDisposable, ISmtpMailSender
{
    private bool _disposed;
    private readonly SmtpClient _smtpClient;
    private readonly MailOptions _mailOptions;

    public SmtpMailSender(SmtpClient smtpClient, IOptions<MailOptions> mailOptions)
    {
        _smtpClient = smtpClient;
        _mailOptions = mailOptions.Value;

    }

    public void SendMessage(MimeMessage message)
    {
        _smtpClient.ClientCertificates = null;
        _smtpClient.CheckCertificateRevocation = false;
        _smtpClient.Connect(_mailOptions.Server, _mailOptions.Port, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
        _smtpClient.Authenticate(_mailOptions.UserName, _mailOptions.Password);
        _smtpClient.Send(message);
        _smtpClient?.Disconnect(true);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (this._disposed)
        {
            return;
        }

        if (disposing)
        {
            _smtpClient?.Dispose();
        }

        this._disposed = true;
    }
}
