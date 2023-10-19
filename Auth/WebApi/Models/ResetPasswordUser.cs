namespace WebApi.Models
{
    public class ResetPasswordUser : BaseUser
    {
        public string Password { get; set; } = default!;
        public string ConfirmedPassword { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
