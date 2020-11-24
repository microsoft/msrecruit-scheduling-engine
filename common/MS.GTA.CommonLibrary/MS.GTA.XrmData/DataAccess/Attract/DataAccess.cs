//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.XrmData.DataAccess.Attract
{
    using MS.GTA.Common.Base.Security.V2;
    using MS.GTA.Common.Data.DataAccess;
    using MS.GTA.Common.EnvironmentSettings;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.Data;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.XrmData.Query;

    public partial class DataAccess : IDataAccess
    {
        /// <summary>The query handler.</summary>
        private readonly IQuery query;

        private readonly IHCMPrincipalRetriever principalRetriever;

        private readonly IEnvironmentMapRepository environmentMapRepository;

        private readonly IEnvironmentSettingProvider environmentSettingsProvider;

        /// <summary>Initializes a new instance of the <see cref="DataAccess"/> class.</summary>
        /// <param name="query">The query handler.</param>
        /// <param name="trace">The trace source.</param>
        /// <param name="principalRetriever">The principal retriever.</param>
        /// <param name="environmentMapRepository">The environment map repository.</param>
        /// <param name="environmentSettingsProvider">The environment settings provider.</param>
        /// <param name="syncServiceNotification">Sync Service notification</param>
        public DataAccess(
            IQuery query,
            ITraceSource trace,
            IHCMPrincipalRetriever principalRetriever,
            IEnvironmentMapRepository environmentMapRepository,
            IEnvironmentSettingProvider environmentSettingsProvider)
        {
            Contract.CheckValue(query, nameof(query));
            Contract.CheckValue(trace, nameof(trace));
            Contract.CheckValue(principalRetriever, nameof(principalRetriever));
            Contract.CheckValue(environmentMapRepository, nameof(environmentMapRepository));
            Contract.CheckValue(environmentSettingsProvider, nameof(environmentSettingsProvider));

            this.query = query;
            this.Trace = trace;
            this.principalRetriever = principalRetriever;
            this.environmentMapRepository = environmentMapRepository;
            this.environmentSettingsProvider = environmentSettingsProvider;
        }

        /// <summary>
        /// Gets or sets the tracer instance
        /// </summary>
        private ITraceSource Trace { get; }
    }
}
