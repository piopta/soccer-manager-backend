namespace GraphQLApi.Validation
{
    public class EditAcademyFacilityInputValidator : AbstractValidator<EditAcademyFacilityInput>
    {
        public EditAcademyFacilityInputValidator()
        {
            RuleFor(p => p.SecondTeamName);
            RuleFor(p => p.ManagerQuality).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
            RuleFor(p => p.FacilitiesQuality).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5);
        }
    }
}
