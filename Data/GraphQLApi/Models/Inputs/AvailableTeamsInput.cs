namespace GraphQLApi.Models.Inputs
{
    public class AvailableTeamsInput
    {
        public Guid TeamId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
