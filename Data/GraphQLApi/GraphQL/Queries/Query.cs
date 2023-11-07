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
        public LeagueModel? GetLeague(Guid leagueId, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Leagues.FirstOrDefault(s => s.Id == leagueId);
        }

        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<CalendarEventModel> GetCalendar(Guid teamId, int year, int month, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            IQueryable<CalendarEventModel> data = ctx.Calendars.Where(c => c.TeamId == teamId && c.Year == year && c.Month == month);

            foreach (CalendarEventModel? d in data)
            {
                d.NotEditable = d.NotEditable ? d.NotEditable : new DateOnly(d.Year, d.Month, d.Day) < DateOnly.FromDateTime(DateTime.UtcNow);
            }

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
        [UseFiltering]
        public IQueryable<MatchModel> GetMatches(Guid id, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Matches.Where(t => t.Id == id);
        }
    }
}
