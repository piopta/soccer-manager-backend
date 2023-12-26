namespace GraphQLApi.Services
{
    public interface IAcademyService
    {
        Task<AcademyPayload> ManagePlayerAcademy(ManageAcademyInput input);
    }
}