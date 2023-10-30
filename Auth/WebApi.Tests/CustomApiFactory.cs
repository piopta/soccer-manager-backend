using DotNet.Testcontainers.Builders;
using Meziantou.Extensions.Logging.Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Npgsql;
using Respawn;
using Respawn.Graph;
using Testcontainers.PostgreSql;
using WebApi.Data;
using WebApi.Models.Options;
using WebApi.Services;
using WebApi.Tests.MockServices;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace WebApi.Tests;

public class CustomApiFactory : WebApplicationFactory<AssemblyMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _sqlContainer =
        new PostgreSqlBuilder()
            .WithImage("postgres:14")
            .WithUsername("postgres")
            .WithPassword("max123456")
            .WithWaitStrategy(Wait.ForUnixContainer().UntilPortIsAvailable(5432))
            .Build();

    private readonly ITestOutputHelper _testOutputHelper = default!;
    private NpgsqlConnection _connection = default!;
    private Respawner _respawner = default!;

    public CustomApiFactory()
    {
        _testOutputHelper = new TestOutputHelper();
    }

    public UserManager<ApplicationUser> UserManager { get; private set; } = default!;

    public RoleManager<ApplicationRole> RoleManager { get; private set; } = default!;

    public ApplicationDbContext DbContext { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging((cfg) =>
        {
            cfg.Services.AddSingleton<ILoggerProvider>(serviceProvider => new XUnitLoggerProvider(_testOutputHelper));
        });

        builder.ConfigureServices((services) =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.RemoveAll<ApplicationDbContext>();

            services.AddDbContext<ApplicationDbContext>((opts) =>
            {
                opts.UseNpgsql(_sqlContainer.GetConnectionString());
            });

            services.RemoveAll<IMailService>();
            services.AddScoped<IMailService, MockMailService>();

            services.RemoveAll<FrontendOptions>();
            services.AddSingleton<FrontendOptions>((_) => new() { AppUrl = "/" });

            services.RemoveAll<JwtOptions>();
            services.AddSingleton<JwtOptions>((_) => new()
            {
                Audience = "",
                Issuer = "",
                Key = "thisisAbsolutely123secure!!!KeyCanyoutestit123!!!"
            });

            ServiceProvider serviceProvider = services.BuildServiceProvider();

            ApplicationDbContext _ctx = serviceProvider.GetRequiredService<ApplicationDbContext>();
            RoleManager<ApplicationRole> _roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            UserManager<ApplicationUser> _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            _ctx.Database.Migrate();

            StartRespawner().GetAwaiter().GetResult();

            UserManager = _userManager;
            RoleManager = _roleManager;
            DbContext = _ctx;
        });
    }

    public async Task InitializeAsync()
    {
        await _sqlContainer.StartAsync();
    }

    private async Task StartRespawner()
    {
        _connection = new NpgsqlConnection(_sqlContainer.GetConnectionString());
        await _connection.OpenAsync();

        _respawner = await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            SchemasToInclude = new[]
            {
                "public"
            },
            TablesToIgnore = new Table[] { "__EFMigrationsHistory" },
            DbAdapter = DbAdapter.Postgres
        });
    }

    public async Task ResetDatabaseState()
    {
        await _respawner.ResetAsync(_connection);
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _sqlContainer.StopAsync();
        await _sqlContainer.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
