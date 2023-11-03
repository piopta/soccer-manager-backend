namespace GraphQLApi.Services
{
    public interface IFacilityService
    {
        Task<AcademyFacilityPayload> AddAcademyFacility(AddAcademyFacilityInput input);
        Task<StadiumPayload> AddStadium(AddStadiumInput input);
        Task<StadiumPayload> EditAcademyFacility(Guid academyId, EditAcademyFacilityInput input);
        Task<StadiumPayload> EditStadium(Guid stadiumId, EditStadiumInput input);
    }
}