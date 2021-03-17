namespace Microsoft.CommonDataService.Common.Internal
{
    public interface IActivityContext
    {
        string SessionId { get; }
        string RootActivityId { get; }
        string ActivityVector { get; }
    }
}
