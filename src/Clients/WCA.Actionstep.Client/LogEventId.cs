namespace WCA.Actionstep.Client
{
    /// <summary>
    /// Contains event IDs for logging.
    /// 
    /// Uses an Enum to avoid accidentally using the same ID for two or more events.
    /// </summary>
    public enum LogEventId
    {
        // Debug
        MethodEntry = 10000,
        MethodExit = 10010,
        AttemptToLockTokenSet = 10020,
        TokenSetLocked = 10030,
        CannotRefreshTokenSetDueToExistingLock = 10040,
        WillNotRefreshTokenSet = 10050,
        TokenSetRevoked = 10060,
        TokenSetNotFoundForUpdate = 10070,
    }
}
