//----------------------------------------------------------------------------
// <copyright file="IFalconQueryClient.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Routing.Client
{
    using System.Threading.Tasks;
    using MS.GTA.Common.DocumentDB;

    /// <summary>
    /// Falcon query client used for storing the client across a single request.
    /// </summary>
    public interface IFalconQueryClient
    {
        /// <summary>Get or generate a Falcon client.</summary>
        /// <returns>Falcon Client</returns>
        Task<IHcmDocumentClient> GetFalconClient();

        /// <summary>Get or generate a Falcon client.</summary>
        /// <param name="containerName">Name of the container in database</param>
        /// <param name="databaseName">Name of the database in CosmosDB account</param>
        /// <returns>Falcon Client</returns>
        Task<IHcmDocumentClient> GetFalconClient(string containerName, string databaseName);
    }
}