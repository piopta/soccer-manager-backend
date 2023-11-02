namespace GraphQLApi.GraphQL.Types
{
    public class TeamType : ObjectType<TeamModel>
    {
        protected override void Configure(IObjectTypeDescriptor<TeamModel> descriptor)
        {
            descriptor.Field(f => f.Logo)
                .ResolveWith<TeamResolvers>(r => r.GetLogo(default!, default!)).UseDbContext<AppDbContext>();

            descriptor.Field(f => f.Shirts)
                .ResolveWith<TeamResolvers>(r => r.GetShirts(default!, default!)).UseDbContext<AppDbContext>();
        }
    }
}
