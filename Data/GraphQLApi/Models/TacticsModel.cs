using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class TacticsModel
    {
        [Key]
        public Guid Id { get; set; }

        public string Formation { get; set; } = default!;
    }
}
