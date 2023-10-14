using Coravel;
using FluentValidation;
using HtmlAgilityPack;
using MailKit.Net.Smtp;
using WebApi;
using WebApi.Data;
using WebApi.Jobs;
using WebApi.Services;
using WebApi.ServicesRegistration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<FrontendOptions>(builder.Configuration.GetSection(ApplicationConstants.FrontendOptionsSectionName));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(ApplicationConstants.JwtOptionsSectionName));
builder.Services.Configure<MailOptions>(builder.Configuration.GetSection(ApplicationConstants.MailOptionsSectionName));

builder.Services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddAuthServices();

builder.Services.AddAutoMapper(typeof(AssemblyMarker).Assembly);

builder.Services.AddSingleton<SmtpClient>();
builder.Services.AddSingleton<HtmlDocument>();
builder.Services.AddSingleton<IEmailBodyParser, EmailBodyParser>();
builder.Services.AddSingleton<ISmtpMailSender, SmtpMailSender>();
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddScheduler();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.Services.UseScheduler((s) =>
{
    s.Schedule<DeleteNotConfirmedUsersJob>().Weekly();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
