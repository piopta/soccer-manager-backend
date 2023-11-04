using GraphQLApi.Services;

namespace GraphQLApi.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<TransferPayload> ManagePlayerTransferStatus(ManagePlayerTransferInput input, [Service(ServiceKind.Resolver)] ITransfersService transfersService)
        {
            return await transfersService.ManagePlayerTransferStatus(input);
        }

        public async Task<TransferPayload> BuyPlayer(Guid teamId, BuyPlayerInput input, [Service(ServiceKind.Resolver)] ITransfersService transfersService)
        {
            return await transfersService.BuyPlayer(teamId, input);
        }
    }
}
