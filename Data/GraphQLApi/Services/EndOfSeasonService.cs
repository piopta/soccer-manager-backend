using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.Services
{
    public class EndOfSeasonService
    {
        private readonly AppDbContext _ctx;
        private readonly IScoresService _scoresService;

        public EndOfSeasonService(IDbContextFactory<AppDbContext> ctx, IScoresService scoresService)
        {
            _ctx = ctx.CreateDbContext();
            _scoresService = scoresService;
        }

        public async Task GenerateNewSeason()
        {
            List<LeagueModel> leagues = _ctx.Leagues.Include(l => l.Scores).ToList();

            List<Guid> teamId = leagues
                .Select(l => l.UserId).ToList();

            DateTime tomorrow = DateTime.UtcNow.AddDays(1);

            List<CalendarEventModel> calendarEvents = _ctx.Calendars
                .Where(l => teamId.Contains(l.TeamId))
                .ToList();

            var calendarTeamIds = calendarEvents.Where(s => new DateTime(s.Year, s.Month, s.Day).Date == tomorrow.Date)
                .Select(s => s.TeamId);

            var newSeasonTeams = teamId.Except(calendarTeamIds).ToList();

            foreach (var t in newSeasonTeams)
            {
                LeagueModel lm = leagues.First(l => l.UserId == t);
                foreach (var leagueTeam in lm.Scores)
                {
                    await _scoresService.CreateScores(leagueTeam.TeamId, lm.Id);
                }
                await GenerateTeamSchedule(lm.Id, lm.UserId);
            }
        }

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
