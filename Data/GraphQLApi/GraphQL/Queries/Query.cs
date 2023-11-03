namespace GraphQLApi.GraphQL.Queries
{
    public class Query
    {
        [UseDbContext(typeof(AppDbContext))]
        [UseFiltering]
        [UseSorting]
        public IQueryable<TeamModel> GetTeams([Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Teams;
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
        [UseFiltering]
        [UseSorting]
        public LeagueModel? GetLeague(Guid leagueId, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Leagues.FirstOrDefault(s => s.Id == leagueId);
        }
    }
}
