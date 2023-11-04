namespace GraphQLApi.Services
{
    public interface ISpendingsService
    {
        Task CreateSpendings(Guid teamId);
    }
}