namespace GraphQLApi.Validation
{
    public class AddStadiumInputValidator : AbstractValidator<AddStadiumInput>
    {
        public AddStadiumInputValidator()
        {
            RuleFor(p => p.StadiumName).NotNull();
            RuleFor(p => p.Capacity).GreaterThanOrEqualTo(0).NotNull();
            RuleFor(p => p.SeatQuality).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5).NotNull();
            RuleFor(p => p.FansExtrasQuality).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5).NotNull();
        }
    }
}
