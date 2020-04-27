namespace WCA.Core.Services.DurableFunctions
{
    public enum RuntimeStatus
    {
        Running,
        Pending,
        Failed,
        Canceled,
        Terminated,
        Completed,
    }
}