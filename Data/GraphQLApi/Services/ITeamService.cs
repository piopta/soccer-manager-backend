﻿namespace GraphQLApi.Services
{
    public interface ITeamService
    {
        Task CreateLeagueTeams(Guid leagueId);
        Task CreateMyTeam(TeamModel team);
        Task<TeamTacticsPayload> ModifyTeamTactics(TeamTacticsInput input);
    }
}