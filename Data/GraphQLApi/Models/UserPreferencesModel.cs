using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GraphQLApi.Models
{
    public class UserPreferencesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid UserId { get; set; }
        public string Mode { get; set; } = default!;
        public bool BottomMenu { get; set; }
        public string NavbarColor { get; set; } = default!;
    }
}
