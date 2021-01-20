//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="XrmQueryClient.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------------

namespace MS.GTA.Common.Talent.Data.Clients
{
    using System.Threading.Tasks;
    using CommonDataService.Common.Internal;
    using MS.GTA.ServicePlatform.Tracing;
    using MS.GTA.Common.Base.Security.V2;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.Common.XrmHttp;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The base query.
    /// </summary>
    public partial class XrmQueryClient
    {
        /*
        /// <summary>The user principal.</summary>
        private readonly IHCMUserPrincipal userPrincipal;

        /// <summary>The app principal.</summary>
        private readonly IHCMApplicationPrincipal appPrincipal;

        /// <summary>The hcm service context.</summary>
        private readonly IHCMServiceContext hcmServiceContext;
        */
        private readonly IXrmHttpClientGenerator xrmClientGenerator;

        /// <summary>Initializes a new instance of the <see cref="XrmQueryClient"/> class.</summary>
        /// <param name="logger">The Logger</param>
        /// <param name="hcmPrincipalRetriever">The HCM Principal Retriever.</param>
        /// <param name="hcmServiceContext">The HCM Service Context.</param>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="xrmClientGenerator">Xrm query client.</param>
        public XrmQueryClient(
            ILogger<XrmQueryClient> logger,
            IHCMPrincipalRetriever hcmPrincipalRetriever,
            IHCMServiceContext hcmServiceContext,
            ITraceSource traceSource,
            IXrmHttpClientGenerator xrmClientGenerator)
        {
            Contract.CheckValue(logger, nameof(logger));
            Contract.CheckValue(traceSource, nameof(traceSource));
            Contract.CheckValue(xrmClientGenerator, nameof(xrmClientGenerator));

            this.Logger = logger;
            this.Trace = traceSource;
            this.xrmClientGenerator = xrmClientGenerator;
        }

        /// <summary>Gets or sets the logger</summary>
        internal ILogger<XrmQueryClient> Logger { get; set; }

        /// <summary>
        /// Gets the tracer instance
        /// </summary>
        internal ITraceSource Trace { get; }
        /*
        /// <summary>
        /// Gets the User Principal
        /// </summary>
        internal IHCMUserPrincipal UserPrincipal { get; }

        /// <summary>
        /// Get an XRM OData client for the user, or an admin client if called with an App token.
        /// </summary>
        /// <returns>The client.</returns>
        internal Task<IXrmHttpClient> GetClient() => this.appPrincipal != null ? this.GetAdminClient() : this.xrmClientGenerator.GetXrmHttpClient();
        */

        /// <summary>
        /// Get an XRM OData client as the app itself.
        /// </summary>
        /// <returns>The client.</returns>
        internal Task<IXrmHttpClient> GetAdminClient() => this.xrmClientGenerator.GetAdminXrmHttpClient();
    }
}