using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.Endpoints
{
    public static class Endpoints
    {
        public static async Task<IResult> IntializeTeam([FromBody] AddTeamInput team, [FromServices] IMapper mapper, [FromServices] IDbContextFactory<AppDbContext> ctxFactory)
        {
            return Results.Ok();
        }
    }
}
