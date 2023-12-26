using GraphQLApi.Services;

namespace GraphQLApi.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<AcademyPayload> ManagePlayerAcademy(ManageAcademyInput input, [Service(ServiceKind.Resolver)] IAcademyService academyService)
        {
            return await academyService.ManagePlayerAcademy(input);
        }

        public async Task<AcademyFacilityPayload> AddAcademyFacility(AddAcademyFacilityInput input, [Service(ServiceKind.Resolver)] IFacilityService facilityService)
        {
            return await facilityService.AddAcademyFacility(input);
        }

        public async Task<StadiumPayload> EditAcademyFacility(Guid academyId, EditAcademyFacilityInput input, [Service(ServiceKind.Resolver)] IFacilityService facilityService)
        {
            return await facilityService.EditAcademyFacility(academyId, input);
        }
    }
}
