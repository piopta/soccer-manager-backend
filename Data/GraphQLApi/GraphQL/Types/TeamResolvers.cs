namespace GraphQLApi.GraphQL.Types
{
    public class TeamResolvers
    {
        public LogoModel GetLogo([Parent] TeamModel team, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Logos.FirstOrDefault(l => l.TeamId == team.Id)!;
        }

        public IQueryable<ShirtModel> GetShirts([Parent] TeamModel team, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            return ctx.Shirts.Where(s => s.TeamId == team.Id)!;
        }
    }
}
