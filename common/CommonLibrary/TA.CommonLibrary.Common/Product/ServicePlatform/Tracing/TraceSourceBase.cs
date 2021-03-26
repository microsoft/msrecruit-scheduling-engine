//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using TA.CommonLibrary.ServicePlatform.Tracing;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Context;
using Microsoft.Extensions.Logging;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Documents;
using System.Collections.Generic;
using Microsoft.ApplicationInsights.Extensibility;
using TA.CommonLibrary.Common.Base.Configuration;
using TA.CommonLibrary.Common.Common.Common.Base.Configuration;

namespace TA.CommonLibrary.ServicePlatform.Tracing
{
    /// <summary>
    /// Base class for all trace sources. The class is aware of the current execution context 
    /// which becomes part of the emitted trace events.
    /// </summary>
    /// <remarks>
    /// Derived classes need to implement the Name and DefaultVerbosity properties
    /// </remarks>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public abstract class TraceSourceBase<T> : ITraceSource where T : TraceSourceBase<T>, new()
    {
        private static readonly T instance = new T();

        public abstract string Name { get; }

        public abstract TraceVerbosity Verbosity { get; }

        private TelemetryClient telemetryClient;

        public static T Instance
        {
            get { return instance; }
        }

        /// <summary>
        /// This constructor should only be called once (by the static constructor).
        /// No other instances should be allowed to be created.
        /// In particular, no instances of derived classes should be created directly --
        /// use the <see cref="Instance"/> member for the singleton.
        /// </summary>
        protected TraceSourceBase()
        {
            Contract.Check(instance == null, "Only use of the singleton instance is allowed");
            this.telemetryClient = new TelemetryClient();
            this.telemetryClient.InstrumentationKey = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<LoggingConfiguration>().InstrumentationKey;
        }

        public void TraceFatal(string message)
        {
            if (ShouldTrace(TraceVerbosity.Fatal))
            {
                TraceInternal(TraceVerbosity.Fatal, message);
            }
        }

        public void TraceFatal(string format, params object[] args)
        {
            if (ShouldTrace(TraceVerbosity.Fatal))
            {
                TraceInternal(TraceVerbosity.Fatal, format, args);
            }
        }

        public void TraceError(string message)
        {
            if (ShouldTrace(TraceVerbosity.Error))
            {
                TraceInternal(TraceVerbosity.Error, message);
            }
        }

        public void TraceError(string format, params object[] args)
        {
            if (ShouldTrace(TraceVerbosity.Error))
            {
                TraceInternal(TraceVerbosity.Error, format, args);
            }
        }

        public void TraceWarning(string message)
        {
            if (ShouldTrace(TraceVerbosity.Warning))
            {
                TraceInternal(TraceVerbosity.Warning, message);
            }
        }

        public void TraceWarning(string format, params object[] args)
        {
            if (ShouldTrace(TraceVerbosity.Warning))
            {
                TraceInternal(TraceVerbosity.Warning, format, args);
            }
        }

        public void TraceInformation(string message)
        {
            if (ShouldTrace(TraceVerbosity.Info))
            {
                TraceInternal(TraceVerbosity.Info, message);
            }
        }

        public void TraceInformation(string format, params object[] args)
        {
            if (ShouldTrace(TraceVerbosity.Info))
            {
                TraceInternal(TraceVerbosity.Info, format, args);
            }
        }

        public void TraceVerbose(string message)
        {
            if (ShouldTrace(TraceVerbosity.Verbose))
            {
                TraceInternal(TraceVerbosity.Verbose, message);
            }
        }

        public void TraceVerbose(string format, params object[] args)
        {
            if (ShouldTrace(TraceVerbosity.Verbose))
            {
                TraceInternal(TraceVerbosity.Verbose, format, args);
            }
        }

        public void Trace(TraceVerbosity verbosity, string message)
        {
            if (ShouldTrace(verbosity))
            {
                TraceInternal(verbosity, message);
            }
        }

        public void Trace(TraceVerbosity verbosity, string format, params object[] args)
        {
            if (ShouldTrace(verbosity))
            {
                TraceInternal(verbosity, format, args);
            }
        }

        public bool ShouldTrace(TraceVerbosity level)
        {
            return level <= Verbosity && ServiceContext.Tracing.ShouldTrace(level);
        }

        private void TraceInternal(TraceVerbosity verbosity, string message, params object[] args)
        {
            IDictionary<string, string> properties = new Dictionary<string, string>();
            properties.Add("RootActivityId", ServiceContext.Activity.Current?.RootActivityId.ToString());
            properties.Add("SessionId", ServiceContext.Activity.Current?.SessionId.ToString());            

            switch (verbosity)
            {
                case TraceVerbosity.Fatal:
                    this.telemetryClient.TrackTrace(message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Critical, properties);
                    break;
                case TraceVerbosity.Error:
                    this.telemetryClient.TrackTrace(message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Error, properties);
                    break;
                case TraceVerbosity.Warning:
                    this.telemetryClient.TrackTrace(message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Warning, properties);
                    break;
                case TraceVerbosity.Info:
                    this.telemetryClient.TrackTrace(message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Information, properties);
                    break;
                case TraceVerbosity.Verbose:
                    this.telemetryClient.TrackTrace(message, Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Verbose, properties);
                    break;
                default:
                    Contract.AssertInvalidSwitchValue<TraceVerbosity>(verbosity, nameof(verbosity));
                    break;
            }
        }
    }
}
