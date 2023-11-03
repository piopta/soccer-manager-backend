using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class TrainingModel
    {
        [Key]
        public Guid Id { get; set; }
        public TrainingType? TrainingType { get; set; }
    }
}
