namespace GraphQLApi.Services
{
    public interface IPlayerService
    {
        Task GenerateTeamPlayers(Guid teamId);
    }
}