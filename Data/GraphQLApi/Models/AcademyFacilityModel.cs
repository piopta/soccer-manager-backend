using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLApi.Models
{
    public class AcademyFacilityModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid AcademyId { get; set; }
        public string SecondTeamName { get; set; } = default!;
        public int ManagerQuality { get; set; }
        public int FacilitiesQuality { get; set; }
    }
}
