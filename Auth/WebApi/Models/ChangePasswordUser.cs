namespace WebApi.Models
{
    public class ChangePasswordUser : BaseUser
    {
        public string OldPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string ConfirmedNewPassword { get; set; } = default!;
    }
}
