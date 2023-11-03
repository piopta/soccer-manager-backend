using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class CalendarEventModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TeamId { get; set; }
        public string Description { get; set; } = default!;
        public EventType EventType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public bool NotEditable { get; set; }

        public Guid? MatchId { get; set; }
        public MatchModel? Match { get; set; }
        public Guid? TrainingId { get; set; }
        public TrainingModel? Training { get; set; }
    }
}
