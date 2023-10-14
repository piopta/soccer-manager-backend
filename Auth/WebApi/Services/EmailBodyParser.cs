using HtmlAgilityPack;
using System.Text;

namespace WebApi.Services;

//guide on how to use htmlagilitypack: https://stackoverflow.com/questions/846994/how-to-use-html-agility-pack
public class EmailBodyParser : IEmailBodyParser
{
    private readonly HtmlDocument _htmlDoc;

    public EmailBodyParser(HtmlDocument htmlDoc)
    {
        _htmlDoc = htmlDoc;
    }

    public string ReadEmailBody(MailInfo mailInfo)
    {
        _htmlDoc.Load(mailInfo.TemplatePath);

        if (_htmlDoc.ParseErrors is not null && _htmlDoc.ParseErrors.Any())
        {
            throw new Exception();
        }

        if (_htmlDoc.DocumentNode is not null)
        {
            string html = _htmlDoc.DocumentNode.OuterHtml;

            return PutParametersValues(html, mailInfo);
        }

        return string.Empty;
    }

    private static string PutParametersValues(string html, MailInfo mailInfo)
    {
        StringBuilder st = new(html);

        foreach (var param in mailInfo.Parameters)
        {
            st.Replace(param.Key, param.Value);
        }

        return st.ToString();
    }
}
