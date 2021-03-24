//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.FalconData.Query
{
    using System.Threading.Tasks;
    using CommonLibrary.Common.DocumentDB;

    /// <summary>
    /// Falcon query client used for storing the client across a single request.
    /// </summary>
    public interface IFalconQueryClient
    {
        /// <summary>Get or generate a Falcon client.</summary>
        /// <returns>Falcon Client</returns>
        Task<IHcmDocumentClient> GetFalconClient();

        /// <summary>Get or generate a Falcon client.</summary>
        /// <param name="databaseName">Name of the database in CosmosDB account</param>
        /// <param name="containerName">Name of the container in database</param>
        /// <returns>Falcon Client</returns>
        Task<IHcmDocumentClient> GetFalconClient(string databaseName, string containerName);
    }
}
