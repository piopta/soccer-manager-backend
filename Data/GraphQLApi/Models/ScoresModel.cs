using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class ScoresModel
    {
        [Key]
        public Guid Id { get; set; }
        public int Season { get; set; }
        public int Points { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Lost { get; set; }
        public IList<GameResultType> Form { get; set; } = new List<GameResultType>();
        public Guid TeamId { get; set; }
        public TeamModel Team { get; set; } = default!;
        public Guid LeagueId { get; set; }
    }
}
