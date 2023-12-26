namespace GraphQLApi.Models.Inputs
{
    public class AddUserPreferencesInput
    {
        public Guid UserId { get; set; }
        public string Mode { get; set; } = default!;
        public bool BottomMenu { get; set; }
        public string NavbarColor { get; set; } = default!;
    }
}
