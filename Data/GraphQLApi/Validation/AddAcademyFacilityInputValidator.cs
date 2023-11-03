namespace GraphQLApi.Validation
{
    public class AddAcademyFacilityInputValidator : AbstractValidator<AddAcademyFacilityInput>
    {
        public AddAcademyFacilityInputValidator()
        {
            RuleFor(p => p.SecondTeamName).NotNull();
            RuleFor(p => p.ManagerQuality).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5).NotNull();
            RuleFor(p => p.FacilitiesQuality).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5).NotNull();
        }
    }
}
