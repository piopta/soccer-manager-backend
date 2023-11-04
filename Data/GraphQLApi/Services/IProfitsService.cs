namespace GraphQLApi.Services
{
    public interface IProfitsService
    {
        Task CreateProfit(Guid teamId);
    }
}