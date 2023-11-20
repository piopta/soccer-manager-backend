namespace GraphQLApi.Models.Inputs
{
    public class EditCalendarEventInput
    {
        public string? Description { get; set; }
        public EventType? EventType { get; set; }

        public Guid? RivalTeamId { get; set; }
        public GroundType? Ground { get; set; }
        public int? TrainingType { get; set; }
    }
}
