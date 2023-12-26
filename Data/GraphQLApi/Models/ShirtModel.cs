using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class ShirtModel
    {
        [Key]
        public Guid Id { get; set; }
        public string MainColor { get; set; } = default!;
        public string SecondaryColor { get; set; } = default!;
        public bool IsSecond { get; set; }
        public SoccerShirtType Type { get; set; }
        public Guid TeamId { get; set; }
    }
}
