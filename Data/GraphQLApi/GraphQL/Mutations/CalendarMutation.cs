using GraphQLApi.Services;

namespace GraphQLApi.GraphQL.Mutations
{
    public partial class Mutation
    {
        public async Task<CalendarPayload> AddCalendarEvent(AddCalendarEventInput input, [Service(ServiceKind.Resolver)] ICalendarService calendarService)
        {
            return await calendarService.AddCalendarEvent(input);
        }

        public async Task<CalendarPayload> EditCalendarEvent(Guid id, EditCalendarEventInput input, [Service(ServiceKind.Resolver)] ICalendarService calendarService)
        {
            return await calendarService.EditCalendarEvent(id, input);
        }
    }
}
