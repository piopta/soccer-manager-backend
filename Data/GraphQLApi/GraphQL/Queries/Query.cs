﻿using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.GraphQL.Queries
{
    public class Query
    {
        [UseDbContext(typeof(AppDbContext))]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<TeamModel> GetTeams([Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Teams;
        }

        [UseDbContext(typeof(AppDbContext))]
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<PlayerModel> GetPlayers([Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Players;
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<SpendingModel> GetTeamSpendings(Guid teamId, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Spendings.Where(t => t.TeamId == teamId);
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ProfitModel> GetTeamProfits(Guid teamId, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Profits.Where(t => t.TeamId == teamId);
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public StadiumModel? GetTeamStadium(Guid userId, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Stadiums.FirstOrDefault(s => s.StadiumId == userId);
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public AcademyFacilityModel? GetTeamAcademyFacility(Guid userId, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.AcademyFacilities.FirstOrDefault(s => s.AcademyId == userId);
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<ScoresModel> GetLeague(Guid leagueId, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Scores.Where(s => s.LeagueId == leagueId);
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseProjection]
        [UseFiltering]
        public IQueryable<CalendarEventModel> GetCalendar(Guid teamId, int year, int month, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            IQueryable<CalendarEventModel> data = ctx.Calendars
                .Where(c => c.TeamId == teamId && c.Year == year && c.Month == month);
            return data;
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<PlayerModel> GetTransfers(Guid teamId, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            IQueryable<PlayerModel> res = ctx.Players.Where(p => p.TeamId != teamId && p.IsOnSale);
            return res;
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public UserPreferencesModel? GetUserPreferences(Guid userId, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            IQueryable<UserPreferencesModel> res = ctx.UserPreferences.Where(p => p.UserId == userId);
            return res.FirstOrDefault();
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        public Guid? GetTeamLeagueId(Guid teamId, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Scores.FirstOrDefault(s => s.TeamId == teamId)?.LeagueId;
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        public IQueryable<ShirtModel> GetShirts(Guid teamId, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Shirts.Where(t => t.TeamId == teamId);
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseProjection]
        [UseFiltering]
        public IQueryable<MatchModel> GetMatches(Guid id, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Matches.Where(t => t.Id == id);
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        public CalendarEventModel? GetLatestMatch(Guid teamId, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Calendars
                .Include(m => m.Match)
                .OrderBy(c => new DateTime(c.Year, c.Month, c.Day)).ToList().Where(c => new DateTime(c.Year, c.Month, c.Day) >= DateTime.UtcNow)
                .FirstOrDefault(s => s.Match?.HomeTeamId == teamId || s.Match?.AwayTeamId == teamId);
        }

        [UseDbContext(typeof(AppDbContext))]
        public IQueryable<TeamModel> GetAvailableTeams(AvailableTeamsInput input, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Teams
                .FromSqlRaw(AppConstants.SqlQueries.AvailableTeams, $"{input.Year}-{input.Month}-{input.Day}", input.TeamId);
        }
    }
}
