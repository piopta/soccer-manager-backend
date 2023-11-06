namespace GraphQLApi.Models.Inputs
{
    public class ManageAcademyInput
    {
        public IList<Guid> Ids { get; set; } = new List<Guid>();
        public bool IsInAcademy { get; set; }
    }
}
