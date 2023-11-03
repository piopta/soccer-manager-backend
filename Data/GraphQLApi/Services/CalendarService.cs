using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GraphQLApi.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly AppDbContext _ctx;
        private readonly IMapper _mapper;
        private readonly IValidator<AddCalendarEventInput> _addEventValidator;
        private readonly IValidator<EditCalendarEventInput> _editEventValidator;

        public CalendarService(IDbContextFactory<AppDbContext> ctx, IMapper mapper,
            IValidator<AddCalendarEventInput> addEventValidator,
            IValidator<EditCalendarEventInput> editEventValidator)
        {
            _ctx = ctx.CreateDbContext();
            _mapper = mapper;
            _addEventValidator = addEventValidator;
            _editEventValidator = editEventValidator;
        }

        public async Task<CalendarPayload> AddCalendarEvent(AddCalendarEventInput input)
        {
            ValidationResult validationRes = await _addEventValidator.ValidateAsync(input);

            if (validationRes.IsValid)
            {
                CalendarEventModel calendar = _mapper.Map<CalendarEventModel>(input);

                if (input.EventType == EventType.MATCH)
                {
                    MatchModel match = _mapper.Map<MatchModel>(input);

                    using (IDbContextTransaction transaction = await _ctx.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            await _ctx.Matches.AddAsync(match);

                            calendar.MatchId = match.Id;
                            calendar.Match = match;

                            await _ctx.Calendars.AddAsync(calendar);
                            await _ctx.SaveChangesAsync();

                            await transaction.CommitAsync();

                            return new(input.TeamId);
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return new(Guid.Empty, "Add event operation ended with failure");
                        }
                    }
                }
                else if (input.EventType == EventType.TRAINING)
                {
                    TrainingModel training = _mapper.Map<TrainingModel>(input);

                    using (IDbContextTransaction transaction = await _ctx.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            await _ctx.Trainings.AddAsync(training);

                            calendar.TrainingId = training.Id;
                            calendar.Training = training;

                            await _ctx.Calendars.AddAsync(calendar);
                            await _ctx.SaveChangesAsync();

                            await transaction.CommitAsync();

                            return new(input.TeamId);
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return new(Guid.Empty, "Add event operation ended with failure");
                        }
                    }
                }
            }

            return new(Guid.Empty);
        }

        public async Task<CalendarPayload> EditCalendarEvent(Guid id, EditCalendarEventInput input)
        {
            ValidationResult validationRes = await _editEventValidator.ValidateAsync(input);

            if (validationRes.IsValid)
            {
                CalendarEventModel? calendar = await _ctx.Calendars.FirstOrDefaultAsync(c => c.Id == id);

                if (calendar is null || new DateOnly(calendar.Year, calendar.Month, calendar.Day) < DateOnly.FromDateTime(DateTime.UtcNow) || calendar.NotEditable)
                {
                    return new(id, "Event not editable - cannot change it");
                }

                if (input.EventType == EventType.MATCH)
                {
                    using (IDbContextTransaction transaction = await _ctx.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            MatchModel match = (await _ctx.Matches.FirstOrDefaultAsync(m => m.Id == calendar.MatchId))!;

                            _mapper.Map(input, calendar);
                            _mapper.Map(input, match);

                            await _ctx.SaveChangesAsync();

                            await transaction.CommitAsync();

                            return new(id);
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return new(Guid.Empty, "Edit event operation ended with failure");
                        }
                    }
                }
                else if (input.EventType == EventType.TRAINING)
                {
                    using (IDbContextTransaction transaction = await _ctx.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            TrainingModel training = (await _ctx.Trainings.FirstOrDefaultAsync(m => m.Id == calendar.TrainingId))!;

                            _mapper.Map(input, calendar);
                            _mapper.Map(input, training);

                            await _ctx.SaveChangesAsync();

                            await transaction.CommitAsync();

                            return new(id);
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            return new(Guid.Empty, "Edit event operation ended with failure");
                        }
                    }
                }
            }

            return new(Guid.Empty);
        }
    }
}
