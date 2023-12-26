namespace GraphQLApi.Models.Payloads
{
    public record TransferPayload(Guid PlayerId, string ErrorMessage = "");
}
