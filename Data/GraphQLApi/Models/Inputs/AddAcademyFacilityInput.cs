namespace GraphQLApi.Models.Inputs
{
    public class AddAcademyFacilityInput
    {
        public Guid AcademyId { get; set; }
        public string SecondTeamName { get; set; } = default!;
        public int ManagerQuality { get; set; }
        public int FacilitiesQuality { get; set; }
    }
}
