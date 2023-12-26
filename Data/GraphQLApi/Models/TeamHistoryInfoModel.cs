using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class TeamHistoryInfoModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public TeamModel Team { get; set; } = default!;
        public Guid PlayerId { get; set; }
        public PlayerModel Player { get; set; } = default!;
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
    }
}
