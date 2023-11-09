using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLApi.Models
{
    public class MatchModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public Guid HomeTeamId { get; set; }
        public Guid AwayTeamId { get; set; }
        public GroundType? Ground { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public TeamModel HomeTeam { get; set; } = default!;
        public TeamModel AwayTeam { get; set; } = default!;
    }
}
