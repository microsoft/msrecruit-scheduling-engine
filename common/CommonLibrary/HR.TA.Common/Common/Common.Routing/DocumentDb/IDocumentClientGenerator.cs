//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Routing.DocumentDb
{
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;
    using Constants;
    using DocumentDB;
    using DocumentDB.Configuration;

    /// <summary>The RoutingDocumentClientGenerator interface.</summary>
    public interface IDocumentClientGenerator
    {
        Task<IDocumentClient> GetDocumentClient(DocumentDBStorageConfiguration documentDbStorageConfiguration, string docDbEntityObjectId = null);
        Task<IDocumentClient> GetHcmGlobalDocumentClient();
        Task<IHcmDocumentClient> GetHcmDocumentClient();
        Task<IHcmDocumentClient> GetGTAHcmDocumentClient();
        Task<IHcmDocumentClient> GetGTAHcmDocumentClient(string containerName, string databaseName);
        Task<IHcmDocumentClient> GetHcmGlobalHcmDocumentClient(string collectionName, string databaseName = "HCMDatabase");
        Task<IDocumentClient> GetHcmRegionalDocumentClient();
        Task<IDocumentClient> GetHcmRegionalDocumentClient(string tenantId, string environmentId, string userObjectId = null, string resourceName = null);
        Task<IHcmDocumentClient> GetHcmRegionalHcmDocumentClient(string tenantId, string environmentId, string databaseId, string collectionId, string partitionKey = null, string userObjectId = null, string resourceName = null);
    }
}
