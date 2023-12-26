namespace GraphQLApi.Models.Payloads
{
    public record AcademyPayload(IList<Guid> PlayerIds, string ErrorMessage = "");
}
