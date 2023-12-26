using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class OpinionModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Opinion { get; set; } = default!;
        public int Rating { get; set; }
    }
}
