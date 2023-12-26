using Microsoft.EntityFrameworkCore;

namespace GraphQLApi.Services
{
    public class MatchSimulationService
    {
        private readonly AppDbContext _ctx;

        public MatchSimulationService(IDbContextFactory<AppDbContext> factory)
        {
            _ctx = factory.CreateDbContext();
        }

        public async Task SimulateMatches()
        {
            DateTime date = DateTime.UtcNow.AddDays(1);

            IEnumerable<CalendarEventModel> calendarEvents = _ctx.Calendars
                .ToList()
                .Where(c => new DateTime(c.Year, c.Month, c.Day).Date == date.Date && c.EventType == EventType.MATCH);

            if (calendarEvents.Any())
            {
                IEnumerable<Guid> matchIds = calendarEvents
                    .Where(c => c.MatchId is not null)
                    .Select(c => (Guid)c.MatchId!);

                List<MatchModel> matches = _ctx.Matches
                    .Where(m => matchIds.Contains(m.Id))
                    .ToList();

                IEnumerable<Guid> teamIds = matches.Select(m => m.HomeTeamId)
                    .Union(matches.Select(m => m.AwayTeamId));

                List<PlayerModel> players = _ctx.Players
                    .Where(p => teamIds.Contains(p.TeamId) && !p.IsBenched && p.SquadPosition > 0)
                    .ToList();

                List<PlayerModel> benchToRestoreEnergyPlayers = _ctx.Players
                    .Where(p => teamIds.Contains(p.TeamId) && p.IsBenched && p.Condition < 100)
                    .ToList();

                List<(Guid teamId, int rating)> teamRatings = players.GroupBy(p => p.TeamId)
                    .Select((group) =>
                    {
                        return CalculateTeamRating(group);
                    })
                    .ToList();

                foreach (PlayerModel player in players)
                {
                    player.Condition -= player.Age;

                    if (player.Condition < 0)
                    {
                        player.InjuredTill = DateTime.UtcNow.AddDays(player.Age - 15);
                    }

                    bool isYellowCard = Random.Shared.Next(0, 100) % 20 == 10;

                    if (isYellowCard)
                    {
                        if (player.YellowCard)
                        {
                            player.Suspended = true;
                        }
                        else
                        {
                            player.YellowCard = true;
                        }
                    }
                }

                foreach (PlayerModel benchedPlayer in benchToRestoreEnergyPlayers)
                {
                    if (benchedPlayer.InjuredTill is null)
                    {
                        benchedPlayer.Condition = 100;
                    }

                    if (benchedPlayer.Suspended)
                    {
                        benchedPlayer.Suspended = false;
                    }

                    if (benchedPlayer.YellowCard)
                    {
                        benchedPlayer.YellowCard = false;
                    }
                }

                List<MatchSimulationResult> simulationResults = new();

                foreach (MatchModel match in matches)
                {
                    (_, int homeTeamRating) = teamRatings.First(t => t.teamId == match.HomeTeamId);
                    (_, int awayTeamRating) = teamRatings.First(t => t.teamId == match.AwayTeamId);

                    if (homeTeamRating > awayTeamRating)
                    {
                        match.HomeScore = Random.Shared.Next(1, 5);
                        match.AwayScore = Random.Shared.Next(0, (int)match.HomeScore - 1!);
                    }
                    else if (homeTeamRating == awayTeamRating)
                    {
                        match.HomeScore = Random.Shared.Next(1, 5);
                        match.AwayScore = match.HomeScore;
                    }
                    else
                    {
                        match.AwayScore = Random.Shared.Next(1, 5);
                        match.HomeScore = Random.Shared.Next(0, (int)match.AwayScore - 1!);
                    }

                    simulationResults.Add(new()
                    {
                        TeamId = match.HomeTeamId,
                        AddedPoints = homeTeamRating > awayTeamRating ? 3 : (homeTeamRating == awayTeamRating ? 1 : 0),
                        Result = homeTeamRating > awayTeamRating ? GameResultType.WIN : (homeTeamRating == awayTeamRating ? GameResultType.DRAW : GameResultType.LOST)
                    });

                    simulationResults.Add(new()
                    {
                        TeamId = match.AwayTeamId,
                        AddedPoints = homeTeamRating > awayTeamRating ? 0 : (homeTeamRating == awayTeamRating ? 1 : 3),
                        Result = homeTeamRating > awayTeamRating ? GameResultType.LOST : (homeTeamRating == awayTeamRating ? GameResultType.DRAW : GameResultType.WIN)
                    });
                }

                List<ScoresModel> scores = _ctx.Scores.Where(s => teamIds.Contains(s.TeamId))
                     .OrderByDescending(s => s.Season)
                     .GroupBy(s => new { s.Season, s.TeamId })
                     .Select(s => s.First())
                     .ToList();

                foreach (ScoresModel score in scores)
                {
                    MatchSimulationResult? simulationResult = simulationResults.FirstOrDefault(s => s.TeamId == score.TeamId);

                    if (simulationResult is not null)
                    {
                        score.Points += simulationResult.AddedPoints;

                        switch (simulationResult.Result)
                        {
                            case GameResultType.WIN:
                                score.Wins++;
                                break;
                            case GameResultType.DRAW:
                                score.Draws++;
                                break;
                            default:
                                score.Lost++;
                                break;
                        }

                        score.Form.Add(simulationResult.Result);
                    }
                }

                await _ctx.SaveChangesAsync();
            }
        }

        private static (Guid teamId, int rating) CalculateTeamRating(IGrouping<Guid, PlayerModel> teamPlayers)
        {
            int playerRatings = teamPlayers.Sum(p => p.SquadRating) ?? 0;
            int maxPlayerRating = teamPlayers.Max(p => p.PlayerRating);
            int averagePlayerPotential = (int)teamPlayers.Average(p => p.PotentialRating);
            int averageCondition = (int)teamPlayers.Average(p => p.Condition) / 10;
            int averageAge = (int)teamPlayers.Average(p => p.Age);
            int randomFactors = Random.Shared.Next(0, 25);

            int teamRating =
                playerRatings + maxPlayerRating + 2 * averagePlayerPotential + averageCondition - 2 * averageAge + randomFactors;

            return (teamPlayers.Key, teamRating);
        }
    }
}
