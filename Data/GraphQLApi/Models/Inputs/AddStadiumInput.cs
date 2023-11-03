namespace GraphQLApi.Models.Inputs
{
    public class AddStadiumInput
    {
        public Guid StadiumId { get; set; }
        public string StadiumName { get; set; } = default!;
        public int Capacity { get; set; }
        public int SeatQuality { get; set; }
        public int FansExtrasQuality { get; set; }
    }
}
