namespace GraphQLApi.Models.Payloads
{
    public record TeamTacticsPayload(Guid TeamId, string ErrorMessage = "");
}
