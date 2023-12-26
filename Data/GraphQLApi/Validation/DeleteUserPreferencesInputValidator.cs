namespace GraphQLApi.Validation
{
    public class DeleteUserPreferencesInputValidator : AbstractValidator<DeleteUserPreferencesInput>
    {
        public DeleteUserPreferencesInputValidator()
        {
            RuleFor(p => p.Id).NotNull();
        }
    }
}
