using AutoMapper;

namespace GraphQLApi.GraphQL.Mutations
{
    public partial class Mutation
    {
        [UseDbContext(typeof(AppDbContext))]
        public async Task<OpinionPayload> AddOpinion(OpinionInput input, [Service(ServiceKind.Resolver)] AppDbContext ctx,
            [Service] IValidator<OpinionInput> validator, [Service] IMapper mapper)
        {
            ValidationResult res = await validator.ValidateAsync(input);

            if (res.IsValid)
            {
                OpinionModel opinion = mapper.Map<OpinionModel>(input);

                await ctx.Opinions.AddAsync(opinion);
                await ctx.SaveChangesAsync();

                return new(opinion.Id);
            }

            return new(Guid.Empty, "Unable to add opinion");
        }
    }
}
