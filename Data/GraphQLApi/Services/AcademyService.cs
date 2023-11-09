using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.Services
{
    public class AcademyService : IAcademyService
    {
        private AppDbContext _ctx;

        public AcademyService(IDbContextFactory<AppDbContext> ctx)
        {
            _ctx = ctx.CreateDbContext();
        }

        public async Task<AcademyPayload> ManagePlayerAcademy(ManageAcademyInput input)
        {
            IEnumerable<PlayerModel> players = _ctx.Players.Where(p => input.Ids.Contains(p.Id)).ToList();

            if (players is not null && players.Any())
            {
                foreach (PlayerModel player in players)
                {
                    player.IsInAcademy = input.IsInAcademy;
                }

                await _ctx.SaveChangesAsync();

                return new(input.Ids);
            }

            return new(Array.Empty<Guid>(), "Player with the given Id doesn't exist");
        }
    }
}
