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
    .AddSorting();

builder.Services.AddAutoMapper(typeof(ApiMarker).Assembly);
builder.Services.AddScoped<IFacilityService, FacilityService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();

builder.Services.AddFluentValidation(opts =>
{
    opts.RegisterValidatorsFromAssembly(typeof(ApiMarker).Assembly);
});


var app = builder.Build();

app.MapGet("/", () => Results.Redirect("/graphql"));

app.MapGraphQL("/graphql");
app.MapGraphQLVoyager("/ui/voyager", new()
{
    GraphQLEndPoint = "/graphql"
});

app.Run();
