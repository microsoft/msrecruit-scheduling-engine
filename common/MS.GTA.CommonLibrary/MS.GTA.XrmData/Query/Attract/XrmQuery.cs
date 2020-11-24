//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.XrmData.Query
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Common.Base.Security.V2;
    using Common.Base.ServiceContext;
    using Common.Routing.DocumentDb;
    using Common.XrmHttp;
    using CommonDataService.Common.Internal;
    using ServicePlatform.Azure.AAD;
    using ServicePlatform.Configuration;
    using ServicePlatform.Tracing;

    /// <summary>
    /// Partial class Xrm Query
    /// </summary>
    /// <seealso cref="MS.GTA.XrmData.Query.IQuery" />
    public partial class XrmQuery : IQuery
    {
        /// <summary>The user principal.</summary>
        private readonly IHCMUserPrincipal userPrincipal;

        /// <summary>The app principal.</summary>
        private readonly IHCMApplicationPrincipal appPrincipal;

        /// <summary>The hcm service context.</summary>
        private readonly IHCMServiceContext hcmServiceContext;

        private readonly IXrmHttpClientGenerator xrmQueryClient;

        /// <summary>The document client generator.</summary>
        private readonly IDocumentClientGenerator documentClientGenerator;

        /// <summary>The azure active directory configuration.</summary>
        private readonly AzureActiveDirectoryClientConfiguration azureActiveDirectoryConfiguration;

        /// <summary>The Logger</summary>
        private ILogger<XrmQuery> logger;

        /// <summary>Specifies whether the query layer should override admin generation logic and force use of an admin context</summary>
        private bool enableAdminModeOverride = false;

        /// <summary>Specifies whether the query layer should override admin generation logic and force use of the admin client combined with user impersonation</summary>
        private Guid? impersonatedUserObjectId = null;

        /// <summary>Initializes a new instance of the <see cref="XrmQuery"/> class.</summary>
        /// <param name="logger">The Logger</param>
        /// <param name="configurationManager">The configuration manager</param>
        /// <param name="hcmPrincipalRetriever">The HCM Principal Retriever.</param>
        /// <param name="hcmServiceContext">The HCM Service Context.</param>
        /// <param name="traceSource">The trace source.</param>
        /// <param name="xrmQueryClient">Xrm query client.</param>
        /// <param name="documentClientGenerator">The Document client generator.</param>
        public XrmQuery(
            ILogger<XrmQuery> logger,
            IConfigurationManager configurationManager,
            IHCMPrincipalRetriever hcmPrincipalRetriever,
            IHCMServiceContext hcmServiceContext,
            ITraceSource traceSource,
            IXrmHttpClientGenerator xrmQueryClient,
            IDocumentClientGenerator documentClientGenerator)
        {
            Contract.CheckValue(logger, nameof(logger));
            Contract.CheckValue(configurationManager, nameof(configurationManager));
            Contract.CheckValue(hcmPrincipalRetriever, nameof(hcmPrincipalRetriever));
            Contract.CheckValue(hcmServiceContext, nameof(hcmServiceContext));
            Contract.CheckValue(traceSource, nameof(traceSource));
            Contract.CheckValue(xrmQueryClient, nameof(xrmQueryClient));
            Contract.CheckValue(documentClientGenerator, nameof(documentClientGenerator));

            this.logger = logger;
            this.userPrincipal = hcmPrincipalRetriever.Principal as IHCMUserPrincipal;
            this.appPrincipal = hcmPrincipalRetriever.Principal as IHCMApplicationPrincipal;
            this.hcmServiceContext = hcmServiceContext;
            this.Trace = traceSource;
            this.xrmQueryClient = xrmQueryClient;
            this.documentClientGenerator = documentClientGenerator;
            this.azureActiveDirectoryConfiguration = configurationManager.Get<AzureActiveDirectoryClientConfiguration>();
        }

        /// <summary>
        /// Gets or sets the tracer instance
        /// </summary>
        private ITraceSource Trace { get; }

        public void EnableAdminOverrideMode()
        {
            this.logger.LogInformation("XrmQuery.EnableAdminOverrideMode: Enabling admin override mode. All XRM queries are being made with admin permissions");
            this.enableAdminModeOverride = true;
        }

        public void EnableUserImpersonationMode(Guid? userObjectId)
        {
            this.impersonatedUserObjectId = userObjectId;
        }

        /// <summary>
        /// Get entity by record id.
        /// </summary>
        /// <typeparam name="T">The type of ODataEntity to get.</typeparam>
        /// <param name="recId">The record id.</param>
        /// <param name="select">Selection function for selecting additional fields.</param>
        /// <param name="expand">Expansion function for expanding additional object properties.</param>
        /// <returns>The entity record.</returns>
        public async Task<T> GetEntityByRecId<T>(Guid recId, Expression<Func<T, object>> select = null, Expression<Func<T, object>> expand = null) where T : ODataEntity
        {
            try
            {
                return await (await this.GetClient()).Get(recId, select: select, expand: expand).ExecuteAndGetAsync("HcmAttXrmGetEntityRecId");
            }
            catch (XrmHttpClientNotFoundResponseException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get service bus message post-image or get the current state by record id.
        /// </summary>
        /// <typeparam name="T">The type of ODataEntity to get.</typeparam>
        /// <param name="remoteExecutionContext">The XRM remote execution context.</param>
        /// <returns>The entity record.</returns>
        public async Task<T> GetPostImageOrCurrent<T>(Microsoft.Xrm.Sdk.RemoteExecutionContext remoteExecutionContext) where T : ODataEntity
        {
            var record = await this.GetEntityByRecId<T>(remoteExecutionContext.PrimaryEntityId);
            record = record ?? remoteExecutionContext.GetPreImage<T>();
            record = record ?? remoteExecutionContext.GetTarget<T>();
            record = record ?? remoteExecutionContext.GetPostImage<T>();

            if (record != null)
            {
                foreach (var preImage in remoteExecutionContext.PreEntityImages)
                {
                    preImage.Value.CopyAttributesOnto(record);
                }
                remoteExecutionContext.GetTargetEntity()?.CopyAttributesOnto(record);
                foreach (var postImage in remoteExecutionContext.PostEntityImages)
                {
                    postImage.Value.CopyAttributesOnto(record);
                }
            }

            return record;
        }

        /// <summary>
        /// Get service bus message target or get the current state by record id.
        /// </summary>
        /// <typeparam name="T">The type of ODataEntity to get.</typeparam>
        /// <param name="remoteExecutionContext">The XRM remote execution context.</param>
        /// <returns>The entity record.</returns>
        public async Task<T> GetTargetOrCurrent<T>(Microsoft.Xrm.Sdk.RemoteExecutionContext remoteExecutionContext) where T : ODataEntity
        {
            return remoteExecutionContext.GetTarget<T>() ?? await this.GetEntityByRecId<T>(remoteExecutionContext.PrimaryEntityId);
        }

        /// <summary>
        /// Get an XRM OData client for the user, or an admin client if called with an App token.
        /// </summary>
        /// <returns>The client.</returns>
        private async Task<IXrmHttpClient> GetClient()
        {
            this.logger.LogInformation($"GetClient: appPrincipal?.ApplicationId={this.appPrincipal?.ApplicationId} enableAdminModeOverride={this.enableAdminModeOverride} impersonatedUserObjectId={this.impersonatedUserObjectId != null}");
            if (this.UseAdminClientForNonAdminQueries())
            {
                this.logger.LogInformation($"GetClient: Building admin client");
                return await this.GetAdminClient();
            }
            else if (this.appPrincipal != null && this.impersonatedUserObjectId != null)
            {
                this.logger.LogInformation($"GetClient: Building user impersonation client");
                return await this.xrmQueryClient.GetUserImpersonationXrmHttpClient(this.impersonatedUserObjectId.Value);
            }
            else
            {
                this.logger.LogInformation($"GetClient: Building user client");
                return await this.xrmQueryClient.GetXrmHttpClient();
            }
        }

        /// <summary>
        /// Get an XRM OData client as the app itself.
        /// </summary>
        /// <returns>The client.</returns>
        private Task<IXrmHttpClient> GetAdminClient() => this.xrmQueryClient.GetAdminXrmHttpClient();

        /// <summary>
        /// Whether to use the admin client for GetClient().
        /// </summary>
        /// <returns>True, if the admin client should be used.</returns>
        private bool UseAdminClientForNonAdminQueries()
        {
            return (this.appPrincipal != null && this.appPrincipal.ApplicationId == this.azureActiveDirectoryConfiguration.ClientId)
                || this.enableAdminModeOverride;
        }
    }
}
