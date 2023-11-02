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
    }
}
