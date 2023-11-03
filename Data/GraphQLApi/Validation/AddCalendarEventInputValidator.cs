namespace GraphQLApi.Validation
{
    public class AddCalendarEventInputValidator : AbstractValidator<AddCalendarEventInput>
    {
        public AddCalendarEventInputValidator()
        {
            RuleFor(p => p.TeamId).NotNull();
            RuleFor(p => p.Description).NotNull();
            RuleFor(p => p.EventType).NotNull();
            RuleFor(p => p.Year).NotNull();
            RuleFor(p => p.Month).NotNull();
            RuleFor(p => p.Day).NotNull();
        }
    }
}
