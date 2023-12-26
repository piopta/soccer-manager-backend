using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.Services
{
    public class ProfitsService : IProfitsService
    {
        private readonly AppDbContext _ctx;

        public ProfitsService(IDbContextFactory<AppDbContext> ctx)
        {
            _ctx = ctx.CreateDbContext();
        }

        public async Task CreateProfit(Guid teamId)
        {
            ProfitModel? profits = await _ctx.Profits.FirstOrDefaultAsync(s => s.TeamId == teamId);

            ProfitModel newProfits = new()
            {
                TeamId = teamId,
                Stadium = 0,
                Transfers = 0,
                Season = profits is null ? 1 : profits.Season + 1
            };

            await _ctx.Profits.AddAsync(newProfits);
            await _ctx.SaveChangesAsync();
        }
    }
}
