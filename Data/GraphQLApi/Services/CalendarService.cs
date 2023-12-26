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
                CalendarEventModel? existingEvent = await _ctx.Calendars
                    .FirstOrDefaultAsync(c => c.TeamId == input.TeamId
                        && c.Day == input.Day && c.Month == input.Month && c.Year == input.Year && c.EventType != EventType.NONE);

                CalendarEventModel? eventToFill = await _ctx.Calendars
                    .FirstOrDefaultAsync(c => c.TeamId == input.TeamId
                        && c.Day == input.Day && c.Month == input.Month && c.Year == input.Year && c.EventType == EventType.NONE);

                if (existingEvent is not null)
                {
                    return new(Guid.Empty, "An event with the chosen day exists");
                }

                if (eventToFill is null)
                {
                    return new(Guid.Empty, "Invalid date - tried to add event outside the season");
                }

                if (input.EventType == EventType.MATCH)
                {
                    MatchModel match = _mapper.Map<MatchModel>(input);
                    match.HomeTeamId = input.TeamId;
                    match.Type = Models.MatchType.FRIENDLY;

                    using (IDbContextTransaction transaction = await _ctx.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            await _ctx.Matches.AddAsync(match);

                            eventToFill.MatchId = match.Id;
                            eventToFill.Match = match;
                            eventToFill.EventType = EventType.MATCH;

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
                    TrainingModel training = new()
                    {
                        TrainingType = (TrainingType)(input.TrainingType ?? 0)
                    };

                    using (IDbContextTransaction transaction = await _ctx.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            await _ctx.Trainings.AddAsync(training);

                            eventToFill.TrainingId = training.Id;
                            eventToFill.Training = training;
                            eventToFill.EventType = EventType.TRAINING;

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
