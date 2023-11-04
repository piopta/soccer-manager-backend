using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.Services
{
    public class AcademyService : IAcademyService
    {
        private readonly IMapper _mapper;
        private AppDbContext _ctx;

        public AcademyService(IDbContextFactory<AppDbContext> ctx, IMapper mapper)
        {
            _ctx = ctx.CreateDbContext();
            _mapper = mapper;
        }

        public async Task<AcademyPayload> ManagePlayerAcademy(ManageAcademyInput input)
        {
            PlayerModel? player = await _ctx.Players.FirstOrDefaultAsync(p => p.Id == input.Id);

            if (player is not null)
            {
                _mapper.Map(input, player);
                await _ctx.SaveChangesAsync();

                return new(input.Id, "Academy player status updated");
            }

            return new(Guid.Empty, "Player with the given Id doesn't exist");
        }
    }
}
