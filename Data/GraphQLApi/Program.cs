using FluentValidation.AspNetCore;
using GraphQLApi;
using GraphQLApi.Extensions;
using GraphQLApi.GraphQL.Mutations;
using GraphQLApi.GraphQL.Queries;
using GraphQLApi.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<AppDbContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("MainConn"));
});

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddFiltering()
    .AddProjections()
    .AddSorting();

builder.Services.AddAutoMapper(typeof(ApiMarker).Assembly);
builder.Services.AddScoped<IFacilityService, FacilityService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();
builder.Services.AddScoped<IScoresService, ScoresService>();
builder.Services.AddScoped<IProfitsService, ProfitsService>();
builder.Services.AddScoped<ISpendingsService, SpendingsService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<ITransfersService, TransfersService>();
builder.Services.AddScoped<IAcademyService, AcademyService>();
builder.Services.AddScoped<MatchSimulationService>();

builder.Services.AddFluentValidation(opts =>
{
    opts.RegisterValidatorsFromAssembly(typeof(ApiMarker).Assembly);
});

builder.Services.AddCors((opts) =>
{
    opts.AddPolicy("AllowFrontendPolicy", pb =>
    {
        pb.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowCredentials()
            .AllowAnyHeader();
    });
});

builder.AddHangfireServices();

var app = builder.Build();

app.UseCors("AllowFrontendPolicy");

if (builder.Environment.IsDevelopment())
{
    app.UseHangfireDashboard();
}

app.Lifetime.ApplicationStarted.Register(() =>
{
    RecurringJob.AddOrUpdate<MatchSimulationService>("simulate-match",
        (matchSimulationService) => matchSimulationService.SimulateMatches(), Cron.Daily(21));

    RecurringJob.AddOrUpdate<EndOfSeasonService>("season-service",
        (endOfSeasonService) => endOfSeasonService.GenerateNewSeason(), Cron.Daily(21, 15));
});

app.MapGet("/", () => Results.Redirect("/graphql"));

app.MapGraphQL("/graphql");
app.MapGraphQLVoyager("/ui/voyager", new()
{
    GraphQLEndPoint = "/graphql"
});

app.MapHangfireDashboard("/hangfire");

app.Run();