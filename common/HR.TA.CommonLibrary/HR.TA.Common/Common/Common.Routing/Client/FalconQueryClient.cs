//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Routing.Client
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using HR.TA.Common.Base.Security.V2;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.Common.DocumentDB;
    using HR.TA.Common.Routing.DocumentDb;
    using HR.TA.ServicePlatform.Tracing;


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

        /// <summary>
        /// The document client dictionary
        /// </summary>
        private static Dictionary<string, IHcmDocumentClient> _documentClientDictionary = new Dictionary<string, IHcmDocumentClient>();

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
        }

        /// <summary>Get or generate a Falcon Client.</summary>
        /// <returns>Falcon Client</returns>
        public async Task<IHcmDocumentClient> GetFalconClient()
        {
            return await GetFalconClient(Constants.DefaultCollectionName, "");
        }

        /// <summary>Get or generate a Falcon client.</summary>
        /// <param name="containerName">Name of the container in database</param>
        /// <param name="databaseName">Name of the database in CosmosDB account</param>
        /// <returns>Falcon Client</returns>
        public async Task<IHcmDocumentClient> GetFalconClient(string containerName, string databaseName)
        {
            string key = GetKey(containerName, databaseName);
            if (_documentClientDictionary.ContainsKey(key))
            {
                return _documentClientDictionary[key];
            }
            else if (containerName == Constants.DefaultCollectionName)
            {
                var _documentClient = await this.documentClientGenerator.GetGTAHcmDocumentClient();
                _documentClientDictionary[key] = _documentClient;
                return _documentClient;
            }
            else
            {
                var _documentClient = await this.documentClientGenerator.GetGTAHcmDocumentClient(containerName, databaseName);
                _documentClientDictionary[key] = _documentClient;
                return _documentClient;
            }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <param name="containerName">Name of the container.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <returns>Key</returns>
        private string GetKey(string containerName, string databaseName)
        {
            return containerName + "-" + databaseName;
        }
    }
}
