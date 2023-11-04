using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class TeamModel
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public Guid UserId { get; set; }
        public int DayOfCreation { get; set; }
        public double Budget { get; set; }
        public Guid LogoId { get; set; }
        public LogoModel Logo { get; set; } = default!;

        public IList<ScoresModel> Scores { get; set; } = new List<ScoresModel>();
        public IList<ShirtModel> Shirts { get; set; } = new List<ShirtModel>();
        public IList<ProfitModel> Profits { get; set; } = new List<ProfitModel>();
        public IList<SpendingModel> Spendings { get; set; } = new List<SpendingModel>();
        public IList<CalendarEventModel> Calendar { get; set; } = new List<CalendarEventModel>();
        public IList<PlayerModel> Players { get; set; } = new List<PlayerModel>();
    }
}
