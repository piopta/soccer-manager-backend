using FluentValidation.AspNetCore;
using GraphQLApi;
using GraphQLApi.GraphQL.Mutations;
using GraphQLApi.GraphQL.Queries;
using GraphQLApi.GraphQL.Types;
using GraphQLApi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextFactory<AppDbContext>(opts =>
{
    opts.UseNpgsql(builder.Configuration.GetConnectionString("MainConn"));
});

builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddType<TeamType>()
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

var app = builder.Build();

app.UseCors("AllowFrontendPolicy");

app.MapGet("/", () => Results.Redirect("/graphql"));

app.MapGraphQL("/graphql");
app.MapGraphQLVoyager("/ui/voyager", new()
{
    GraphQLEndPoint = "/graphql"
});

app.Run();
