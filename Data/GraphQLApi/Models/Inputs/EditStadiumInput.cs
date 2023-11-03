namespace GraphQLApi.Models.Inputs
{
    public class EditStadiumInput
    {
        public string? StadiumName { get; set; }
        public int? Capacity { get; set; }
        public int? SeatQuality { get; set; }
        public int? FansExtrasQuality { get; set; }
    }
}
