using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLApi.Models
{
    public class StadiumModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid StadiumId { get; set; }
        public string StadiumName { get; set; } = default!;
        public int Capacity { get; set; }
        public int SeatQuality { get; set; }
        public int FansExtrasQuality { get; set; }
    }
}
