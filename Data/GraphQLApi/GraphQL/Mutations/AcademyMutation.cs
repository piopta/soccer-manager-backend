using GraphQLApi.Services;

namespace GraphQLApi.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<AcademyPayload> ManagePlayerAcademy(ManageAcademyInput input, [Service(ServiceKind.Resolver)] IAcademyService academyService)
        {
            return await academyService.ManagePlayerAcademy(input);
        }
    }
}
