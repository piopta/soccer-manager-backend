using GraphQLApi.Services;

namespace GraphQLApi.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<StadiumPayload> AddStadium(AddStadiumInput input, [Service(ServiceKind.Resolver)] IFacilityService facilityService)
        {
            return await facilityService.AddStadium(input);
        }

        public async Task<StadiumPayload> EditStadium(Guid stadiumId, EditStadiumInput input, [Service(ServiceKind.Resolver)] IFacilityService facilityService)
        {
            return await facilityService.EditStadium(stadiumId, input);
        }
    }
}
