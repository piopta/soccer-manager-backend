namespace WebApi.Models
{
    public class LoginUser : BaseUser
    {
        public string Password { get; set; } = default!;
    }
}
