namespace GraphQLApi.Validation
{
    public class EditCalendarEventInputValidator : AbstractValidator<EditCalendarEventInput>
    {
        public EditCalendarEventInputValidator()
        {
            RuleFor(p => p.Description).NotNull();
            RuleFor(p => p.EventType).NotNull();
        }
    }
}
