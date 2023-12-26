using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class SpendingModel
    {
        [Key]
        public Guid Id { get; set; }
        public int Season { get; set; }
        public double? Transfers { get; set; }
        public double? Salaries { get; set; }
        public Guid TeamId { get; set; }
    }
}
