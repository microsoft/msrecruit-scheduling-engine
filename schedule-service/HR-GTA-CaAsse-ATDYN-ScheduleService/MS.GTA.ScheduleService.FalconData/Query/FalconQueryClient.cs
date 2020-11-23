//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="FalconQueryClient.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.FalconData.Query
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Base.Security.V2;
    using MS.GTA.Common.Base.ServiceContext;
    using MS.GTA.Common.DocumentDB;
    using MS.GTA.Common.Routing.DocumentDb;

    /// <summary>
    /// Falcon query Client used for storing the Client across a single request.
    /// </summary>
    public class FalconQueryClient : IFalconQueryClient
    {
        /// <summary>The user principal.</summary>
        private readonly IHCMUserPrincipal userPrincipal;

        /// <summary>The hcm service context.</summary>
        private readonly IHCMServiceContext hcmServiceContext;

        /// <summary>The document client generator.</summary>
        private readonly IDocumentClientGenerator documentClientGenerator;

        /// <summary>The Logger</summary>
        private readonly ILogger<FalconQueryClient> logger;

        private readonly Dictionary<string, IHcmDocumentClient> falconConnections;

        /// <summary>
        /// Initializes a new instance of the <see cref="FalconQueryClient"/> class.
        /// </summary>
        /// <param name="logger">The Logger</param>
        /// <param name="documentClientGenerator">The Document client generator.</param>
        /// <param name="hcmPrincipalRetriever">The HCM Principal Retriever.</param>
        /// <param name="hcmServiceContext">The HCM Service Context.</param>
        public FalconQueryClient(
            ILogger<FalconQueryClient> logger,
            IDocumentClientGenerator documentClientGenerator,
            IHCMPrincipalRetriever hcmPrincipalRetriever,
            IHCMServiceContext hcmServiceContext)
        {
            this.logger = logger;
            this.userPrincipal = hcmPrincipalRetriever.Principal as IHCMUserPrincipal;
            this.hcmServiceContext = hcmServiceContext;
            this.documentClientGenerator = documentClientGenerator;
            this.falconConnections = new Dictionary<string, IHcmDocumentClient>();
        }

        /// <summary>Get or generate a Falcon Client.</summary>
        /// <returns>Falcon Client connection</returns>
        public async Task<IHcmDocumentClient> GetFalconClient()
        {
            if (!this.falconConnections.ContainsKey("Default"))
            {
                this.logger.LogInformation("GetFalconClient: Establishing default connection");
                this.falconConnections.Add("Default", await this.documentClientGenerator.GetHcmDocumentClient());
            }

            return this.falconConnections["Default"];
        }

        /// <summary>Get or generate a Falcon client.</summary>
        /// <param name="databaseName">Name of the database in CosmosDB account</param>
        /// <param name="collectionName">Name of the container in database</param>
        /// <returns>Falcon Client</returns>
        public async Task<IHcmDocumentClient> GetFalconClient(string databaseName, string collectionName)
        {
            if (!this.falconConnections.ContainsKey(collectionName))
            {
                this.logger.LogInformation("GetFalconClient: Establishing connection for " + databaseName + " and " + collectionName);
                this.falconConnections.Add(collectionName, await this.documentClientGenerator.GetGTAHcmDocumentClient(collectionName, databaseName));
            }

            return this.falconConnections[collectionName];
        }
    }
}
