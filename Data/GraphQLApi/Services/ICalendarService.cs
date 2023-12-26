namespace GraphQLApi.Services
{
    public interface ICalendarService
    {
        Task<CalendarPayload> AddCalendarEvent(AddCalendarEventInput input);
        Task<CalendarPayload> EditCalendarEvent(Guid id, EditCalendarEventInput input);
    }
}