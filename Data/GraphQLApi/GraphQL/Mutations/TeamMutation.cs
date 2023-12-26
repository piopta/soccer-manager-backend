

using AutoMapper;
using GraphQLApi.Services;
using Microsoft.EntityFrameworkCore.Storage;

namespace GraphQLApi.GraphQL.Mutations
{
    public partial class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        public async Task<TeamTacticsPayload> ModifyTeamTactics(TeamTacticsInput input,
            [Service(ServiceKind.Resolver)] ITeamService teamService, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            var squadPlayersIds = input.SquadPlayers.Select(s => s.Id).ToList();
            var anyInvalidPlayers = ctx.Players
                .Where(p => squadPlayersIds.Contains(p.Id) && (p.InjuredTill != null || p.Suspended) && p.IsBenched && p.SquadPosition > 0)
                .Any();

            if (anyInvalidPlayers)
            {
                return new(Guid.Empty, "");
            }

            return await teamService.ModifyTeamTactics(input);
        }

        [UseDbContext(typeof(AppDbContext))]
        public async Task<AddTeamPayload> AddTeam(AddTeamInput input, [Service(ServiceKind.Resolver)] AppDbContext ctx,
            [Service] IMapper mapper, [Service(ServiceKind.Resolver)] ITeamService teamService, CancellationToken token)
        {
            TeamModel team = mapper.Map<TeamModel>(input);
            LogoModel logo = mapper.Map<LogoModel>(input);
            ShirtModel firstShirt = mapper.Map<ShirtModel>(input);
            ShirtModel secondShirt = new()
            {
                MainColor = input.SecondMainColor,
                SecondaryColor = input.SecondSecondaryColor,
                Type = input.SecondType,
                IsSecond = true
            };

            using (IDbContextTransaction transaction = await ctx.Database.BeginTransactionAsync(token))
            {
                try
                {
                    team.Id = input.UserId;
                    team.Formation = "4-3-3";
                    await ctx.Teams.AddAsync(team, token);
                    logo.TeamId = team.Id;
                    firstShirt.TeamId = team.Id;
                    secondShirt.TeamId = team.Id;

                    await ctx.Logos.AddAsync(logo, token);
                    await ctx.Shirts.AddAsync(firstShirt, token);
                    await ctx.Shirts.AddAsync(secondShirt, token);

                    team.LogoId = logo.Id;
                    team.Budget = 5_000_000;

                    await ctx.SaveChangesAsync(token);

                    await transaction.CommitAsync(token);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync(token);
                }
            }
            await teamService.CreateMyTeam(team);
            return new(team.Id);
        }
    }
}
