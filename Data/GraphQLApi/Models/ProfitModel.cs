using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class ProfitModel
    {
        [Key]
        public Guid Id { get; set; }
        public int Season { get; set; }
        public double? Transfers { get; set; }
        public double? Stadium { get; set; }
        public Guid TeamId { get; set; }
    }
}
