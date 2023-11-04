using AutoMapper;
using Bogus;
using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.Services
{
    public class TeamService : ITeamService
    {
        private readonly AppDbContext _ctx;
        private readonly ICalendarService _calendarService;
        private readonly IFacilityService _facilityService;
        private readonly IScoresService _scoresService;
        private readonly IProfitsService _profitsService;
        private readonly ISpendingsService _spendingsService;
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;

        private readonly Faker<TeamModel> _teamsGenerator = new Faker<TeamModel>()
            .RuleFor(t => t.Name, g => $"{g.Company.CompanyName()} {g.Address.City()}")
            .RuleFor(t => t.Logo, g => new LogoModel()
            {
                IconId = g.Random.Number(1, 6).ToString(),
                MainColor = g.PickRandom(new[] { "red", "green", "blue", "black", "white" }),
                SecondaryColor = g.PickRandom(new[] { "red", "green", "blue", "black", "white" }),
                Type = g.Random.Enum<SoccerShirtType>()
            });

        public TeamService(IDbContextFactory<AppDbContext> ctx, ICalendarService calendarService,
            IFacilityService facilityService, IScoresService scoresService, IProfitsService profitsService,
            ISpendingsService spendingsService, IPlayerService playerService,
            IMapper mapper)
        {
            _ctx = ctx.CreateDbContext();
            _calendarService = calendarService;
            _facilityService = facilityService;
            _scoresService = scoresService;
            _profitsService = profitsService;
            _spendingsService = spendingsService;
            _playerService = playerService;
            _mapper = mapper;
        }

        public async Task CreateMyTeam(TeamModel team)
        {
            Guid leagueId = Guid.NewGuid();

            LeagueModel league = new()
            {
                Id = leagueId,
                UserId = team.UserId,
                Name = $"{team.Name} league"
            };

            await _ctx.Leagues.AddAsync(league);
            await _ctx.SaveChangesAsync();

            await _scoresService.CreateScores(team.Id, leagueId);
            await _profitsService.CreateProfit(team.Id);
            await _spendingsService.CreateSpendings(team.Id);
            await _playerService.GenerateTeamPlayers(team.Id);

            await CreateLeagueTeams(leagueId);
            await GenerateTeamSchedule(leagueId);
        }

        public async Task CreateLeagueTeams(Guid leagueId)
        {
            for (int i = 0; i < 9; i++)
            {
                TeamModel tm = _teamsGenerator.Generate();

                tm.UserId = Guid.Empty;
                tm.DayOfCreation = DateTime.UtcNow.Day;

                await _ctx.Teams.AddAsync(tm);
                tm.Logo.TeamId = tm.Id;

                await _ctx.Logos.AddAsync(tm.Logo);
                tm.LogoId = tm.Logo.Id;
                await _ctx.SaveChangesAsync();

                await _scoresService.CreateScores(tm.Id, leagueId);
                await _profitsService.CreateProfit(tm.Id);
                await _spendingsService.CreateSpendings(tm.Id);
                await _playerService.GenerateTeamPlayers(tm.Id);
            }
        }

        private async Task GenerateTeamSchedule(Guid leagueId)
        {
            List<MatchModel> matches = new();
            List<ScoresModel> teamScores = _ctx.Scores.Where(s => s.LeagueId == leagueId).ToList();

            DateTime dateNow = DateTime.UtcNow;

            List<ScoresModel> teamsToGenerateSchedule = teamScores.Skip(1).ToList();
            int teamsListSize = teamsToGenerateSchedule.Count();
            int halfTeams = teamScores.Count() / 2;

            for (int i = 0; i < teamScores.Count() - 1; i++)
            {
                int teamId = i % teamsListSize;

                MatchModel match = new()
                {
                    HomeTeamId = teamScores[0].TeamId,
                    AwayTeamId = teamsToGenerateSchedule[teamId].TeamId,
                    Ground = Random.Shared.Next(1, 2) % 2 == 0 ? GroundType.HOME : GroundType.AWAY
                };

                for (int j = 0; j < halfTeams - 1; j++)
                {
                    int firstTeam = (i + j) % teamsListSize;
                    int secondTeam = (teamsListSize - 1 - j + i) % teamsListSize;

                    MatchModel matchInner = new()
                    {
                        HomeTeamId = teamsToGenerateSchedule[firstTeam].TeamId,
                        AwayTeamId = teamsToGenerateSchedule[secondTeam].TeamId,
                        Ground = Random.Shared.Next(1, 2) % 2 == 0 ? GroundType.HOME : GroundType.AWAY
                    };
                    matches.Add(matchInner);
                }

                matches.Add(match);
            }

            await _ctx.Matches.AddRangeAsync(matches);
            await _ctx.SaveChangesAsync();
        }
    }
}
