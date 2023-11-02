using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLApi.Models
{
    public class TeamModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public Guid UserId { get; set; }
        public Guid LogoId { get; set; }

        [NotMapped]
        public LogoModel Logo { get; set; } = default!;

        public IList<ScoresModel> Scores { get; set; } = new List<ScoresModel>();
        public IList<ShirtModel> Shirts { get; set; } = new List<ShirtModel>();
    }
}
