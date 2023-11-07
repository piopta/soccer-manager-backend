namespace GraphQLApi.Models.Inputs
{
    public class OpinionInput
    {
        public Guid UserId { get; set; }
        public string Opinion { get; set; } = default!;
        public int Rating { get; set; }
    }
}
