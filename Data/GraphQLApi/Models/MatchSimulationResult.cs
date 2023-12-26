namespace GraphQLApi.Models
{
    public class MatchSimulationResult
    {
        public Guid TeamId { get; set; }
        public int AddedPoints { get; set; }
        public GameResultType Result { get; set; }
    }
}
