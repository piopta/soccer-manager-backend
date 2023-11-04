namespace GraphQLApi.Services
{
    public interface ITransfersService
    {
        Task<TransferPayload> BuyPlayer(Guid teamId, BuyPlayerInput input);
        Task<TransferPayload> ManagePlayerTransferStatus(ManagePlayerTransferInput input);
    }
}