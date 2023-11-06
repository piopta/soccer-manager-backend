using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GraphQLApi.Services
{
    public class TransfersService : ITransfersService
    {
        private readonly AppDbContext _ctx;
        private readonly IMapper _mapper;

        public TransfersService(IDbContextFactory<AppDbContext> ctx, IMapper mapper)
        {
            _ctx = ctx.CreateDbContext();
            _mapper = mapper;
        }

        public async Task<TransferPayload> ManagePlayerTransferStatus(ManagePlayerTransferInput input)
        {
            PlayerModel? player = await _ctx.Players.FirstOrDefaultAsync(p => p.Id == input.Id);

            if (player is not null)
            {
                _mapper.Map(input, player);
                await _ctx.SaveChangesAsync();

                return new(input.Id);
            }

            return new(Guid.Empty, "Player with the given Id doesn't exist");
        }

        public async Task<TransferPayload> BuyPlayer(Guid teamId, BuyPlayerInput input)
        {
            PlayerModel? player = await _ctx.Players.FirstOrDefaultAsync(p => p.Id == input.Id && p.IsOnSale);

            if (player is not null)
            {
                using (IDbContextTransaction transaction = await _ctx.Database.BeginTransactionAsync())
                {
                    try
                    {
                        TeamModel? buyingTeam = await _ctx.Teams.FirstOrDefaultAsync(t => t.Id == teamId);
                        TeamModel playerTeam = (await _ctx.Teams.FirstOrDefaultAsync(t => t.Id == player.TeamId))!;


                        if (buyingTeam is null)
                        {
                            return new(player.Id, "Team with the given Id doesn't exist");
                        }

                        if (buyingTeam.Id == playerTeam.Id)
                        {
                            return new(player.Id, "Trying to perform transfer operation within one team");
                        }

                        player.TeamId = buyingTeam.Id;
                        player.Team = buyingTeam;
                        player.IsOnSale = false;
                        TeamHistoryInfoModel historyInfoModel = new()
                        {
                            TeamId = playerTeam.Id,
                            PlayerId = player.Id,
                            From = DateTime.UtcNow
                        };

                        SpendingModel spending = await _ctx.Spendings.OrderBy(s => s.Id).LastAsync(s => s.TeamId == buyingTeam.Id);
                        ProfitModel profit = await _ctx.Profits.OrderBy(s => s.Id).LastAsync(s => s.TeamId == playerTeam.Id);

                        spending.Transfers += player.MarketValue;
                        profit.Transfers += player.MarketValue;

                        await _ctx.TeamHistories.AddAsync(historyInfoModel);
                        await _ctx.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return new(player.Id);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }

            return new(Guid.Empty, "Player with the given Id isn't on sale");
        }
    }
}
