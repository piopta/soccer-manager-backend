using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class LeagueModel
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; } = default!;

        public IList<ScoresModel> Scores { get; set; } = new List<ScoresModel>();
    }
}
