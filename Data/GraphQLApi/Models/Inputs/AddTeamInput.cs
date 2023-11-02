namespace GraphQLApi.Models.Inputs
{
    public class AddTeamInput
    {
        public string Name { get; set; } = default!;
        public Guid UserId { get; set; }
        public string LogoMainColor { get; set; } = default!;
        public string LogoSecondaryColor { get; set; } = default!;
        public string IconId { get; set; } = default!;
        public SoccerShirtType LogoType { get; set; }
        public string FirstMainColor { get; set; } = default!;
        public string FirstSecondaryColor { get; set; } = default!;
        public SoccerShirtType FirstType { get; set; }
        public string SecondMainColor { get; set; } = default!;
        public string SecondSecondaryColor { get; set; } = default!;
        public SoccerShirtType SecondType { get; set; }
    }
}
