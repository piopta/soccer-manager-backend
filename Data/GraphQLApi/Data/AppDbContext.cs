using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace GraphQLApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TeamModel> Teams => Set<TeamModel>();
        public DbSet<LogoModel> Logos => Set<LogoModel>();
        public DbSet<ShirtModel> Shirts => Set<ShirtModel>();
        public DbSet<ScoresModel> Scores => Set<ScoresModel>();
        public DbSet<LeagueModel> Leagues => Set<LeagueModel>();
        public DbSet<UserPreferencesModel> UserPreferences => Set<UserPreferencesModel>();

        //https://stackoverflow.com/questions/65328620/ef-core-list-of-enums
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var valueComparer = new ValueComparer<IList<GameResultType>>(
                    (c1, c2) => c1.SequenceEqual(c2),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => (IList<GameResultType>)c.ToHashSet());

            modelBuilder.Entity<ScoresModel>()
                .Property(e => e.Form)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList().Select(v => Enum.Parse<GameResultType>(v)).ToList())
                .Metadata.SetValueComparer(valueComparer);

        }
    }
}
