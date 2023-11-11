namespace GraphQLApi
{
    public static class AppConstants
    {
        public static class StadiumCosts
        {
            public const double CapacityUpdate = 0.1;
            public const double SeatQualityUpdate = 100_000;
            public const double FansExtrasUpdate = 10_000;
        }

        public static class AcademyFacilityCosts
        {
            public const double ManagerQualityUpdate = 1_000_000;
            public const double FacilitiesQualityUpdate = 200_000;
        }

        public static class SqlQueries
        {
            public const string AvailableTeams = @"SELECT t.* FROM public.""Teams"" t
                                                                    EXCEPT
                                                                    SELECT DISTINCT t.* FROM public.""Teams"" t
                                                                    INNER JOIN public.""Matches"" m
                                                                    ON t.""Id"" = m.""AwayTeamId"" OR t.""Id"" = m.""HomeTeamId""
                                                                    INNER JOIN public.""Calendars"" c 
                                                                    ON m.""Id"" = c.""MatchId""
                                                                    WHERE CONCAT(c.""Year"",'-',c.""Month"",'-',c.""Day"") = {0} AND t.""Id"" != {1};";
        }

        public static readonly string[] Logos = new[] { "gi-falcon", "gi-castle-1", "gi-horse", "gi-flamer", "gi-chicken" };
        public static readonly string[] Colors = new[] { "red", "green", "blue", "black", "tomato", "aqua", "orange" };
    }
}
