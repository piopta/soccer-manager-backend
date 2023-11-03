using GraphQLApi.Data.ValueComparers;
using Microsoft.EntityFrameworkCore;

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
        public DbSet<SpendingModel> Spendings => Set<SpendingModel>();
        public DbSet<ProfitModel> Profits => Set<ProfitModel>();
        public DbSet<StadiumModel> Stadiums => Set<StadiumModel>();
        public DbSet<AcademyFacilityModel> AcademyFacilities => Set<AcademyFacilityModel>();
        public DbSet<MatchModel> Matches => Set<MatchModel>();
        public DbSet<TrainingModel> Trainings => Set<TrainingModel>();
        public DbSet<CalendarEventModel> Calendars => Set<CalendarEventModel>();

        //https://stackoverflow.com/questions/65328620/ef-core-list-of-enums
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ScoresModel>()
                .Property(e => e.Form)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList().Select(v => Enum.Parse<GameResultType>(v)).ToList())
                .Metadata.SetValueComparer(new GameResultTypeListComparer());

        }
    }
}
