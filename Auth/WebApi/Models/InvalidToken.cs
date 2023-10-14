using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class InvalidToken
    {
        [Key]
        public Guid Id { get; set; }
        public string TokenHash { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
        public string UserId { get; set; } = default!;
    }
}
