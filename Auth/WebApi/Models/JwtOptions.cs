namespace WebApi.Models
{
    public class JwtOptions
    {
        public string Audience { get; set; } = default!;

        public string Issuer { get; set; } = default!;

        public int? ClockSkew { get; set; }

        public string Key { get; set; } = default!;
    }
}
