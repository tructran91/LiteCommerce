namespace Identity.Core.Constants
{
    // Shared by the log EF configurations and the code that truncates values before saving.
    public static class LogFieldLength
    {
        public const int Action = 30;
        public const int FailureReason = 30;
        public const int EntityName = 100;
        public const int EntityId = 200;
        public const int Email = 256;
        public const int Description = 500;
        public const int IpAddress = 45;
        public const int UserAgent = 512;
        public const int UserName = 256;
        public const int CorrelationId = 100;
    }
}
