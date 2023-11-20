namespace GraphQLApi.Models.Inputs
{
    public class TeamTacticsInput
    {
        public Guid TeamId { get; set; }
        public IList<PlayerTacticsPositionModel> SquadPlayers { get; set; } = new List<PlayerTacticsPositionModel>();
        public IList<string> BenchPlayers { get; set; } = new List<string>();
        public string Formation { get; set; } = default!;
    }
}
