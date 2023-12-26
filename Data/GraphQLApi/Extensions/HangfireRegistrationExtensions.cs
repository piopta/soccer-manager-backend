using Hangfire;
using Hangfire.PostgreSql;

namespace GraphQLApi.Extensions
{
    public static class HangfireRegistrationExtensions
    {
        public static void AddHangfireServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddHangfire(cfg =>
            {
                cfg.UsePostgreSqlStorage((opts) =>
                {
                    opts.UseNpgsqlConnection(builder.Configuration.GetConnectionString("MainConn"));
                });
            });
            builder.Services.AddHangfireServer();
        }
    }
}
