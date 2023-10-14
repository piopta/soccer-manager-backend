namespace WebApi.Models
{
    public class ApplicationUser : IdentityUser
    {
        public IList<InvalidToken> InvalidTokens { get; set; } = default!;
    }
}
