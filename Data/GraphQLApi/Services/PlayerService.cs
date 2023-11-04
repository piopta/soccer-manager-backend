﻿using Bogus;
using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly AppDbContext _ctx;
        private readonly Faker<PlayerModel> _playerGenerator = new Faker<PlayerModel>()
            .RuleFor(p => p.PlayerName, g => g.Person.FullName)
            .RuleFor(p => p.PlayerRating, g => g.Random.Number(1, 5))
            .RuleFor(p => p.PotentialRating, g => g.Random.Number(1, 6))
            .RuleFor(p => p.PositionType, g => g.Random.Enum<PositionType>())
            .RuleFor(p => p.PlayerNumber, g => g.Random.Number(1, 100))
            .RuleFor(p => p.Image, g => g.Image.People())
            .RuleFor(p => p.CountryCode, g => g.Address.CountryCode())
            .RuleFor(p => p.Foot, g => g.Random.ArrayElement(new string[] { "L", "R" }))
            .RuleFor(p => p.Age, g => g.Random.Number(16, 35))
            .RuleFor(p => p.Wage, g => g.Random.Number(10_000, 30_000))
            .RuleFor(p => p.ContractTo, g => g.Date.Between(DateTime.UtcNow, DateTime.UtcNow.AddYears(5)));

        public PlayerService(IDbContextFactory<AppDbContext> ctx)
        {
            _ctx = ctx.CreateDbContext();
        }

        public async Task GenerateTeamPlayers(Guid teamId)
        {
            List<PlayerModel> players = _playerGenerator.GenerateBetween(20, 22);

            players.ForEach(p =>
            {
                p.TeamId = teamId;
                p.MarketValue = p.Wage * 5 + p.PlayerRating * 100_000 - (p.Age * 1_000);
            });

            List<PlayerModel> academyPlayers = _playerGenerator.GenerateBetween(2, 4);

            academyPlayers.ForEach(p =>
            {
                p.TeamId = teamId;
                p.MarketValue = p.Wage * 5 + p.PlayerRating * 100_000 - (p.Age * 1_000);
                p.IsInAcademy = true;
                p.PlayerRating = 1;
            });

            await _ctx.Players.AddRangeAsync(players);
            await _ctx.Players.AddRangeAsync(academyPlayers);
            await _ctx.SaveChangesAsync();
        }
    }
}
