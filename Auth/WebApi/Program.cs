using FluentValidation;
using HtmlAgilityPack;
using MailKit.Net.Smtp;
using WebApi;
using WebApi.Data;
using WebApi.Services;
using WebApi.ServicesRegistration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(ApplicationConstants.JwtOptionsSectionName));
builder.Services.Configure<MailOptions>(builder.Configuration.GetSection(ApplicationConstants.MailOptionsSectionName));

builder.Services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();
builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.AddAuthServices();

builder.Services.AddSingleton<SmtpClient>();
builder.Services.AddSingleton<HtmlDocument>();
builder.Services.AddSingleton<IEmailBodyParser, EmailBodyParser>();
builder.Services.AddSingleton<ISmtpMailSender, SmtpMailSender>();
builder.Services.AddScoped<IMailService, MailService>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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

var roleManager = app.Services.GetRequiredService<RoleManager<ApplicationRole>>();

if ((await roleManager.FindByNameAsync(ApplicationConstants.Roles.Admin) is null))
{
    await roleManager.CreateAsync(new() { Name = ApplicationConstants.Roles.Admin });
    await roleManager.CreateAsync(new() { Name = ApplicationConstants.Roles.User });
}
