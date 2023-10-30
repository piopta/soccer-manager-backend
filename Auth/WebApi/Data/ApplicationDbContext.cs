using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts,
            IConfiguration configuration) : base(opts)
        {
            _configuration = configuration;
        }

        public DbSet<InvalidToken> InvalidTokens { get; set; } = default!;
    }
}
