namespace WebApi.Models
{
    //inspired here: https://mailtrap.io/blog/asp-net-core-send-email/#:~:text=How%20to%20send%20email%20using%20SMTP%20from%20ASP.NET,Step%20%236%20%E2%80%93%20Write%20the%20email-sending%20code%20
    public class MailOptions
    {
        public string Server { get; set; } = default!;
        public int Port { get; set; }
        public string SenderEmail { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
