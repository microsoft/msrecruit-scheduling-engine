namespace Microsoft.CommonDataService.Common.Internal
{
    public interface IMonitoredError
    {
        string ErrorCode { get; }
        string ErrorNamespace { get; }
    }
}
