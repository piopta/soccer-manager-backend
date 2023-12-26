namespace WebApi.Models.Results
{
    public class LoginUserResult
    {
        public string UserId { get; set; } = default!;
        public string Token { get; set; } = default!;
    }
}
