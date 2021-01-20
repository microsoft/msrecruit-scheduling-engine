namespace MS.GTA.ServicePlatform.Tracing
{
    /// <summary>
    /// Interface for a source of traces.
    /// Contains methods to fire traces.
    /// Also contains an ID, verbosity of the source and method to determine if traces
    /// should be fired in a given verbosity for the current transaction.
    /// </summary>
    /// <remarks>
    /// An ITraceSource is the smallest unit of control for traces.
    /// All traces from the same source have common verbosity configuration. 
    /// </remarks>
    public interface ITraceSource : IIdentifiable
    {
        void Trace(TraceVerbosity verbosity, string message);

        void Trace(TraceVerbosity verbosity, string format, params object[] args);

        void TraceFatal(string message);

        void TraceFatal(string format, params object[] args);

        void TraceError(string message);

        void TraceError(string format, params object[] args);

        void TraceWarning(string message);

        void TraceWarning(string format, params object[] args);

        void TraceInformation(string message);

        void TraceInformation(string format, params object[] args);

        void TraceVerbose(string message);

        void TraceVerbose(string format, params object[] args);

        bool ShouldTrace(TraceVerbosity level);
    }
}
