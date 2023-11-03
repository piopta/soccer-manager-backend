namespace GraphQLApi.Validation
{
    public class EditUserPreferencesInputValidator : AbstractValidator<EditUserPreferencesInput>
    {
        public EditUserPreferencesInputValidator()
        {
            RuleFor(r => r.Mode).Must(BeValidMode);
            RuleFor(r => r.BottomMenu);
            RuleFor(r => r.NavbarColor).Matches("#([0-9]|[A-F]|[a-f]){6}");
        }
        private static bool BeValidMode(string? mode)
        {
            return mode is null || mode == "light" || mode == "dark";
        }
    }
}
