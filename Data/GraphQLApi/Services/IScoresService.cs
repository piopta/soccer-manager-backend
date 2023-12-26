namespace GraphQLApi.Services
{
    public interface IScoresService
    {
        Task CreateScores(Guid teamId, Guid leagueId);
    }
}