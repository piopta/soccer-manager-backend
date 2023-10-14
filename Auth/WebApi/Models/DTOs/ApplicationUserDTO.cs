namespace WebApi.Models.DTOs
{
    public class ApplicationUserDTO
    {
        public string Email { get; set; } = default!;
        public bool LockoutEnabled { get; set; } = default!;
    }
}
