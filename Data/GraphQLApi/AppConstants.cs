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

        public static readonly string[] Logos = new[] { "gi-falcon", "gi-castle-1", "gi-horse", "gi-flamer", "gi-chicken" };
        public static readonly string[] Colors = new[] { "red", "green", "blue", "black", "white" };
    }
}
