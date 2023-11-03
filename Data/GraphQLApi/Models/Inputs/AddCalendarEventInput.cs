namespace GraphQLApi.Models.Inputs
{
    public class AddCalendarEventInput
    {
        public Guid TeamId { get; set; }
        public string Description { get; set; } = default!;
        public EventType EventType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }

        public Guid? RivalTeamId { get; set; }
        public GroundType? Ground { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public TrainingType? TrainingType { get; set; }
    }
}
