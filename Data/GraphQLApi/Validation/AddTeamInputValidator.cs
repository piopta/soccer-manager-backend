using FluentValidation;

namespace GraphQLApi.Validation
{
    public class AddTeamInputValidator : AbstractValidator<AddTeamInput>
    {
        public AddTeamInputValidator()
        {
            RuleFor(r => r.UserId).NotEmpty();
            RuleFor(r => r.IconId).NotNull();
            RuleFor(r => r.LogoType).NotNull();
            RuleFor(r => r.FirstMainColor).NotNull();
            RuleFor(r => r.FirstType).NotNull();
            RuleFor(r => r.SecondMainColor).NotNull();
            RuleFor(r => r.SecondType).NotNull();
        }
    }
}
