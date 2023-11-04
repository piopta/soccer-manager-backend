using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.Services
{
    public class ScoresService : IScoresService
    {
        private readonly AppDbContext _ctx;

        public ScoresService(IDbContextFactory<AppDbContext> ctx)
        {
            _ctx = ctx.CreateDbContext();
        }

        public async Task CreateScores(Guid teamId, Guid leagueId)
        {
            ScoresModel? scores = await _ctx.Scores.FirstOrDefaultAsync(s => s.TeamId == teamId);

            ScoresModel newScores = new()
            {
                TeamId = teamId,
                Draws = 0,
                Wins = 0,
                Lost = 0,
                Points = 0,
                Season = scores is null ? 1 : scores.Season + 1,
                LeagueId = leagueId
            };

            await _ctx.Scores.AddAsync(newScores);
            await _ctx.SaveChangesAsync();
        }
    }
}
