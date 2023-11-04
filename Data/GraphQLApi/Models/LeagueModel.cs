using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLApi.Models
{
    public class LeagueModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Name { get; set; } = default!;

        public IList<ScoresModel> Scores { get; set; } = new List<ScoresModel>();
    }
}
