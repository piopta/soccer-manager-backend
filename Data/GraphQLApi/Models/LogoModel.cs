using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class LogoModel
    {
        [Key]
        public Guid Id { get; set; }
        public string MainColor { get; set; } = default!;
        public string SecondaryColor { get; set; } = default!;
        public string IconId { get; set; } = default!;
        public SoccerShirtType Type { get; set; }
        public Guid TeamId { get; set; }
    }
}
