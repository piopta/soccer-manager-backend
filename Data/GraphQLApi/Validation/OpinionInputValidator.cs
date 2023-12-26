namespace GraphQLApi.Validation
{
    public class OpinionInputValidator : AbstractValidator<OpinionInput>
    {
        public OpinionInputValidator()
        {
            RuleFor(o => o.UserId).NotNull();
            RuleFor(o => o.Opinion).NotEmpty();
            RuleFor(o => o.Rating).LessThanOrEqualTo(5);
        }
    }
}
