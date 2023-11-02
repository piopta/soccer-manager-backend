using FluentValidation;

namespace GraphQLApi.Validation
{
    public class AddUserPreferencesInputValidator : AbstractValidator<AddUserPreferencesInput>
    {
        public AddUserPreferencesInputValidator()
        {
            RuleFor(r => r.Mode).Must(BeValidMode).NotNull();
            RuleFor(r => r.BottomMenu).NotNull();
            RuleFor(r => r.NavbarColor).Matches("#([1-9]|[A-F]|[a-f]){6}").NotNull();
        }
        private static bool BeValidMode(string mode)
        {
            return mode == "light" || mode == "dark";
        }
    }

}
