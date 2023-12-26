namespace GraphQLApi.Validation
{
    public class EditStadiumInputValidator : AbstractValidator<EditStadiumInput>
    {
        public EditStadiumInputValidator()
        {
            RuleFor(p => p.StadiumName);
            RuleFor(p => p.Capacity).GreaterThanOrEqualTo(0);
            RuleFor(p => p.SeatQuality).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
            RuleFor(p => p.FansExtrasQuality).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
        }
    }
}
