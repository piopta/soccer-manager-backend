﻿using System.ComponentModel.DataAnnotations;

namespace GraphQLApi.Models
{
    public class MatchModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HomeTeamId { get; set; }
        public Guid AwayTeamId { get; set; }
        public GroundType? Ground { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
    }
}
