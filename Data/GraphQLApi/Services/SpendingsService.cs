using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.Services
{
    public class SpendingsService : ISpendingsService
    {
        private readonly AppDbContext _ctx;

        public SpendingsService(IDbContextFactory<AppDbContext> ctx)
        {
            _ctx = ctx.CreateDbContext();
        }

        public async Task CreateSpendings(Guid teamId)
        {
            SpendingModel? spendings = await _ctx.Spendings.FirstOrDefaultAsync(s => s.TeamId == teamId);

            SpendingModel newSpendings = new()
            {
                TeamId = teamId,
                Salaries = 0,
                Transfers = 0,
                Season = spendings is null ? 1 : spendings.Season + 1
            };

            await _ctx.Spendings.AddAsync(newSpendings);
            await _ctx.SaveChangesAsync();
        }
    }
}
