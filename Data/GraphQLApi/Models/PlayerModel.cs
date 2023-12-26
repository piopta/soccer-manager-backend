using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class PlayerModel
    {
        [Key]
        public Guid Id { get; set; }

        public string PlayerName { get; set; } = default!;
        public int PlayerRating { get; set; } = default!;
        public int PotentialRating { get; set; } = default!;
        public int? SquadRating { get; set; }
        public PositionType PositionType { get; set; }
        public int SquadPosition { get; set; }
        public int PlayerNumber { get; set; }
        public string Image { get; set; } = default!;
        public int Condition { get; set; } = 100;
        public string CountryCode { get; set; } = default!;
        public string Foot { get; set; } = default!;
        public Guid TeamId { get; set; }
        public TeamModel Team { get; set; } = default!;
        public bool IsBenched { get; set; }
        public bool IsInAcademy { get; set; }
        public DateTime? InjuredTill { get; set; }
        public bool Suspended { get; set; }
        public bool YellowCard { get; set; }
        public bool IsOnSale { get; set; }
        public double Wage { get; set; }
        public double MarketValue { get; set; }
        public DateTime ContractTo { get; set; }
        public int Age { get; set; }
        public IList<TeamHistoryInfoModel> TeamHistory { get; set; } = new List<TeamHistoryInfoModel>();
    }
}
