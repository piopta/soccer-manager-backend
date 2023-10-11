namespace WebApi.Models
{
    public class MailInfo
    {
        public string TemplatePath { get; set; } = default!;
        public string To { get; set; } = default!;
        public string Subject { get; set; } = default!;
        public Dictionary<string, string> Parameters { get; set; } = default!;
    }
}
