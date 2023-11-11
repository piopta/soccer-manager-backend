using AutoMapper;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace GraphQLApi.Services
{
    public class TeamService : ITeamService
    {
        private readonly AppDbContext _ctx;
        private readonly ICalendarService _calendarService;
        private readonly IScoresService _scoresService;
        private readonly IProfitsService _profitsService;
        private readonly ISpendingsService _spendingsService;
        private readonly IPlayerService _playerService;
        private readonly IMapper _mapper;

        private readonly Faker<TeamModel> _teamsGenerator = new Faker<TeamModel>()
            .RuleFor(t => t.Name, g => $"{g.Company.CompanyName()} {g.Address.City()}")
            .RuleFor(t => t.Formation, "4-3-3")
            .RuleFor(t => t.Logo, g => new LogoModel()
            {
                IconId = g.PickRandom(AppConstants.Logos),
                MainColor = g.PickRandom(AppConstants.Colors),
                SecondaryColor = g.PickRandom(AppConstants.Colors),
                Type = g.Random.Enum<SoccerShirtType>()
            });

        private readonly Faker<ShirtModel> _shirtsGenerator = new Faker<ShirtModel>()
            .RuleFor(t => t.MainColor, g => g.PickRandom(AppConstants.Colors))
            .RuleFor(t => t.SecondaryColor, g => g.PickRandom(AppConstants.Colors))
            .RuleFor(t => t.Type, g => g.Random.Enum<SoccerShirtType>());

        public TeamService(IDbContextFactory<AppDbContext> ctx, ICalendarService calendarService,
            IScoresService scoresService, IProfitsService profitsService,
            ISpendingsService spendingsService, IPlayerService playerService,
            IMapper mapper)
        {
            _ctx = ctx.CreateDbContext();
            _calendarService = calendarService;
            _scoresService = scoresService;
            _profitsService = profitsService;
            _spendingsService = spendingsService;
            _playerService = playerService;
            _mapper = mapper;
        }

        public async Task<TeamTacticsPayload> ModifyTeamTactics(TeamTacticsInput input)
        {
            List<PlayerModel> teamPlayers = _ctx.Players.Where(p => p.TeamId == input.TeamId).ToList();
            TeamModel? team = await _ctx.Teams.FirstOrDefaultAsync(t => t.Id == input.TeamId);

            if (teamPlayers.Any())
            {
                using (IDbContextTransaction transaction = await _ctx.Database.BeginTransactionAsync())
                {
                    try
                    {
                        foreach (var squadPlayer in input.SquadPlayers)
                        {
                            var player = teamPlayers.First(p => p.Id == squadPlayer.Id);
                            player.IsBenched = false;
                            player.SquadPosition = squadPlayer.SquadPosition;
                        }

                        foreach (var benchedId in input.BenchPlayers)
                        {
                            var player = teamPlayers.First(p => p.Id == benchedId);
                            player.IsBenched = true;
                            player.SquadPosition = 0;
                        }

                        if (team is not null)
                        {
                            team.Formation = input.Formation;
                        }

                        await _ctx.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return new(input.TeamId);
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                    }
                }

            }

            return new(Guid.Empty, "An error has occured while updating teams' tactics");
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
            await GenerateTeamSchedule(leagueId, team.Id);
        }

        public async Task CreateLeagueTeams(Guid leagueId)
        {
            for (int i = 0; i < 9; i++)
            {
                TeamModel tm = _teamsGenerator.Generate();

                tm.UserId = Guid.NewGuid();
                tm.Id = tm.UserId;
                tm.DayOfCreation = DateTime.UtcNow.Day;

                await _ctx.Teams.AddAsync(tm);
                tm.Logo.TeamId = tm.Id;

                await _ctx.Logos.AddAsync(tm.Logo);
                tm.LogoId = tm.Logo.Id;

                ShirtModel firstShirt = _shirtsGenerator.Generate();
                ShirtModel secondShirt = _shirtsGenerator.Generate();

                firstShirt.TeamId = tm.Id;
                secondShirt.TeamId = tm.Id;
                secondShirt.IsSecond = true;

                await _ctx.Shirts.AddRangeAsync(firstShirt, secondShirt);

                await _ctx.SaveChangesAsync();

                await _scoresService.CreateScores(tm.Id, leagueId);
                await _profitsService.CreateProfit(tm.Id);
                await _spendingsService.CreateSpendings(tm.Id);
                await _playerService.GenerateTeamPlayers(tm.Id);
            }
        }

        //inspired here: https://stackoverflow.com/questions/1037057/how-to-automatically-generate-a-sports-league-schedule
        private async Task GenerateTeamSchedule(Guid leagueId, Guid dbTeamId)
        {
            List<MatchModel> matches = new();
            List<ScoresModel> teamScores = _ctx.Scores.Where(s => s.LeagueId == leagueId).ToList();
            List<CalendarEventModel> calendarEvents = new();

            List<ScoresModel> teamsToGenerateSchedule = teamScores.Skip(1).ToList();
            int teamsListSize = teamsToGenerateSchedule.Count();
            int halfTeams = teamScores.Count() / 2;

            DateTime dateNow = DateTime.UtcNow.AddDays(1);

            for (int k = 0; k < 2; k++)
            {
                for (int i = 0; i < teamScores.Count() - 1; i++)
                {
                    int teamId = i % teamsListSize;

                    MatchModel match = new()
                    {
                        Id = Guid.NewGuid(),
                        HomeTeamId = teamScores[0].TeamId,
                        AwayTeamId = teamsToGenerateSchedule[teamId].TeamId,
                        Ground = Random.Shared.Next(1, 3) % 2 == 0 ? GroundType.HOME : GroundType.AWAY
                    };

                    CalendarEventModel calendarEvent = new()
                    {
                        Day = dateNow.Day,
                        Description = string.Empty,
                        EventType = EventType.MATCH,
                        Match = match,
                        MatchId = match.Id,
                        Month = dateNow.Month,
                        NotEditable = true,
                        Year = dateNow.Year,
                        TeamId = dbTeamId
                    };

                    for (int j = 0; j < halfTeams - 1; j++)
                    {
                        int firstTeam = (i + j) % teamsListSize;
                        int secondTeam = (teamsListSize - 1 - j + i) % teamsListSize;

                        MatchModel matchInner = new()
                        {
                            Id = Guid.NewGuid(),
                            HomeTeamId = teamsToGenerateSchedule[firstTeam].TeamId,
                            AwayTeamId = teamsToGenerateSchedule[secondTeam].TeamId,
                            Ground = Random.Shared.Next(1, 3) % 2 == 0 ? GroundType.HOME : GroundType.AWAY
                        };

                        CalendarEventModel calendarEventInner = new()
                        {
                            Day = dateNow.Day,
                            Description = string.Empty,
                            EventType = EventType.MATCH,
                            Match = matchInner,
                            MatchId = matchInner.Id,
                            Month = dateNow.Month,
                            NotEditable = true,
                            Year = dateNow.Year
                        };

                        matches.Add(matchInner);
                        calendarEvents.Add(calendarEventInner);
                    }

                    matches.Add(match);
                    calendarEvents.Add(calendarEvent);
                    dateNow = dateNow.AddDays(1);
                }

                if (k == 0)
                {
                    List<CalendarEventModel> freeEvents = new();

                    for (int i = 0; i < 3; i++)
                    {
                        var ce = new CalendarEventModel(dateNow.AddDays(i), dbTeamId);
                        freeEvents.Add(ce);
                    }

                    calendarEvents.AddRange(freeEvents);
                }

                dateNow = dateNow.AddDays(3);
            }

            await _ctx.Matches.AddRangeAsync(matches);
            await _ctx.Calendars.AddRangeAsync(calendarEvents);
            await _ctx.SaveChangesAsync();
        }
    }
}
