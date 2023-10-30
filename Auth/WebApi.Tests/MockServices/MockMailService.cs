using OneOf;
using WebApi.Models.Results;
using WebApi.Services;

namespace WebApi.Tests.MockServices;

public class MockMailService : IMailService
{
    public OneOf<bool, ErrorResultGeneral> SendMail(MailInfo mail)
    {
        return true;
    }
}
