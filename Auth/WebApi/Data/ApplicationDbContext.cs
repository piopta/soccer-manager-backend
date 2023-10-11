using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts,
            IConfiguration configuration) : base(opts)
        {
            _configuration = configuration;
        }

        public DbSet<InvalidToken> InvalidTokens { get; set; } = default!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseNpgsql(_configuration.GetConnectionString(ApplicationConstants.DbConnectionStringName));
        }
    }
}
