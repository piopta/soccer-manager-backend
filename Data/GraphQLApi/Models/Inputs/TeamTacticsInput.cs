namespace GraphQLApi.Models.Inputs
{
    public class TeamTacticsInput
    {
        public Guid TeamId { get; set; }
        public IList<PlayerTacticsPositionModel> SquadPlayers { get; set; } = new List<PlayerTacticsPositionModel>();
        public IList<Guid> BenchPlayers { get; set; } = new List<Guid>();
    }
}
