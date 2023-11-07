namespace GraphQLApi.GraphQL.Types
{
    public class TeamResolvers
    {
        public LogoModel GetLogo([Parent] TeamModel team, [Service(ServiceKind.Resolver)] AppDbContext ctx)
        {
            var logo = ctx.Logos.FirstOrDefault(l => l.TeamId == team.Id)!;
            return logo;
        }
    }
}
