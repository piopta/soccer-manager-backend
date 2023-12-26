using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GraphQLApi.Services
{
    public class FacilityService : IFacilityService
    {
        private readonly AppDbContext _ctx;
        private readonly IValidator<AddStadiumInput> _addStadiumValidator;
        private readonly IValidator<EditStadiumInput> _editStadiumValidator;
        private readonly IValidator<AddAcademyFacilityInput> _addAcademyValidator;
        private readonly IValidator<EditAcademyFacilityInput> _editAcademyValidator;
        private readonly IMapper _mapper;

        public FacilityService(IDbContextFactory<AppDbContext> ctx, IValidator<AddStadiumInput> addStadiumValidator,
            IValidator<EditStadiumInput> editStadiumValidator, IValidator<AddAcademyFacilityInput> addAcademyValidator,
            IValidator<EditAcademyFacilityInput> editAcademyValidator, IMapper mapper)
        {
            _ctx = ctx.CreateDbContext();
            _addStadiumValidator = addStadiumValidator;
            _editStadiumValidator = editStadiumValidator;
            _addAcademyValidator = addAcademyValidator;
            _editAcademyValidator = editAcademyValidator;
            _mapper = mapper;
        }

        public async Task<StadiumPayload> AddStadium(AddStadiumInput input)
        {
            ValidationResult validationResult = await _addStadiumValidator.ValidateAsync(input);

            if (validationResult.IsValid)
            {
                StadiumModel stadium = _mapper.Map<StadiumModel>(input);
                TeamModel? team = await _ctx.Teams.FirstOrDefaultAsync(t => t.UserId == input.StadiumId);
                SpendingModel? spending = await _ctx.Spendings.OrderBy(s => s.Season).LastOrDefaultAsync(t => t.TeamId == team.Id);

                if (team is null || spending is null)
                {
                    return new(Guid.Empty, "Add stadium operation ended with failure");
                }

                using (IDbContextTransaction transaction = await _ctx.Database.BeginTransactionAsync())
                {
                    try
                    {
                        UpdateSpendingsStadiumValue(stadium, spending);

                        await _ctx.Stadiums.AddAsync(stadium);
                        await _ctx.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return new(Guid.Empty, "Add stadium operation ended with failure");
                    }
                }

                return new(input.StadiumId);
            }

            return new(Guid.Empty, "Invalid input passed");
        }

        public async Task<StadiumPayload> EditStadium(Guid stadiumId, EditStadiumInput input)
        {
            ValidationResult validationResult = await _editStadiumValidator.ValidateAsync(input);

            if (validationResult.IsValid)
            {
                StadiumModel? stadium = await _ctx.Stadiums.FirstOrDefaultAsync(c => c.StadiumId == stadiumId);

                TeamModel? team = await _ctx.Teams.FirstOrDefaultAsync(t => t.UserId == stadiumId);
                SpendingModel? spending = await _ctx.Spendings.OrderBy(s => s.Season).LastOrDefaultAsync(t => t.TeamId == team.Id);

                if (stadium is null || team is null || spending is null)
                {
                    return new(stadiumId, "Stadium with the given Id doesn't exist");
                }

                using (IDbContextTransaction transaction = await _ctx.Database.BeginTransactionAsync())
                {
                    try
                    {
                        UpdateSpendingsStadiumValue(stadium, spending);

                        _mapper.Map(input, stadium);
                        await _ctx.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return new(Guid.Empty, "Add stadium operation ended with failure");
                    }
                }

                return new(stadiumId);
            }

            return new(Guid.Empty, "Invalid input passed");
        }

        public async Task<AcademyFacilityPayload> AddAcademyFacility(AddAcademyFacilityInput input)
        {
            ValidationResult validationResult = await _addAcademyValidator.ValidateAsync(input);

            if (validationResult.IsValid)
            {
                AcademyFacilityModel academy = _mapper.Map<AcademyFacilityModel>(input);
                TeamModel? team = await _ctx.Teams.FirstOrDefaultAsync(t => t.UserId == input.AcademyId);
                SpendingModel? spending = await _ctx.Spendings.OrderBy(s => s.Season).LastOrDefaultAsync(t => t.TeamId == team.Id);

                if (team is null || spending is null)
                {
                    return new(Guid.Empty, "Add academy facility operation ended with failure");
                }

                using (IDbContextTransaction transaction = await _ctx.Database.BeginTransactionAsync())
                {
                    try
                    {
                        UpdateSpendingsAcademyValue(academy, spending);

                        await _ctx.AcademyFacilities.AddAsync(academy);
                        await _ctx.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        return new(Guid.Empty, "Add academy facility operation ended with failure");
                    }
                }

                return new(input.AcademyId);
            }

            return new(Guid.Empty, "Invalid input passed");
        }

        public async Task<StadiumPayload> EditAcademyFacility(Guid academyId, EditAcademyFacilityInput input)
        {
            ValidationResult validationResult = await _editAcademyValidator.ValidateAsync(input);

            if (validationResult.IsValid)
            {
                AcademyFacilityModel? academy = await _ctx.AcademyFacilities.FirstOrDefaultAsync(c => c.AcademyId == academyId);

                TeamModel? team = await _ctx.Teams.FirstOrDefaultAsync(t => t.UserId == academyId);
                SpendingModel? spending = await _ctx.Spendings.OrderBy(s => s.Season).LastOrDefaultAsync(t => t.TeamId == team.Id);

                if (academy is null || team is null || spending is null)
                {
                    return new(academyId, "Academy facility with the given Id doesn't exist");
                }

                using (IDbContextTransaction transaction = await _ctx.Database.BeginTransactionAsync())
                {
                    try
                    {
                        UpdateSpendingsAcademyValue(academy, spending);

                        _mapper.Map(input, academy);
                        await _ctx.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        return new(Guid.Empty, "Edit academy facility operation ended with failure");
                    }
                }

                return new(academyId);
            }

            return new(Guid.Empty, "Invalid input passed");
        }

        private static void UpdateSpendingsStadiumValue(StadiumModel stadium, SpendingModel spending)
        {
            double cost = stadium.SeatQuality * AppConstants.StadiumCosts.SeatQualityUpdate +
                stadium.FansExtrasQuality * AppConstants.StadiumCosts.FansExtrasUpdate +
                stadium.Capacity * AppConstants.StadiumCosts.CapacityUpdate;

            spending.Salaries += cost;
        }

        private static void UpdateSpendingsAcademyValue(AcademyFacilityModel academy, SpendingModel spending)
        {
            double cost = academy.FacilitiesQuality * AppConstants.StadiumCosts.SeatQualityUpdate +
                academy.ManagerQuality * AppConstants.StadiumCosts.FansExtrasUpdate;

            spending.Salaries += cost;
        }
    }
}
