namespace TrackYourTripGRPCApi.Utilities
{
    public static class SD
    {
        public enum TripStatus
        {
            Planned = 0,
            Ongoing = 1,
            Completed = 2,
            Cancelled = 3
        }

        public const int Success = 200;
        public const int InvalidCredentials = 401;
        public const int NotFound = 404;
        public const int ServerError = 500;

    }
}
