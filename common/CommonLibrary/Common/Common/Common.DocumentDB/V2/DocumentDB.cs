//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.DocumentDB.V2
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Threading.Tasks;

    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Base.KeyVault;
    using Base.Utilities;
    using CommonDataService.Common.Internal;
    using Configuration;
    using Exceptions;
    using Extensions;
    using ServicePlatform.Context;
    using ServicePlatform.Exceptions;
    using ServicePlatform.Tracing;
    using Microsoft.Extensions.Logging;
    using ServicePlatform.Azure.Security;

    /// <summary>
    /// The document database class.
    /// </summary>
    [Obsolete("Please use the new DocumentClientGenerator, it has a much better name and caching for clients. This will be removed in a future release. It should have the same signature.")]
    public class DocumentDB : IDocumentDB
    {
        /// <summary>
        /// Trace source
        /// </summary>
        private readonly ILogger<DocumentDB> logger;

        /// <summary>
        /// Secret manager provider
        /// </summary>
        private readonly ISecretManager secretManager;

        /// <summary>
        /// Keeps a list of document client per document DB url
        /// </summary>
        private ConcurrentDictionary<string, KeyValuePair<string, string>> documentDBConnections;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDB" /> class.
        /// </summary>
        /// <param name="secretManager">The secret manager.</param>
        /// <param name="logger">The logger.</param>
        public DocumentDB(ISecretManager secretManager, ILogger<DocumentDB> logger)
        {
            Contract.CheckValue(logger, nameof(logger));
            Contract.CheckValue(secretManager, nameof(secretManager));

            this.logger = logger;
            this.secretManager = secretManager;
            this.documentDBConnections = new ConcurrentDictionary<string, KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Get document client based on configuration
        /// </summary>
        /// <param name="documentDbConfigurations">DB configurations</param>
        /// <returns>Document Client</returns>
        public async Task<DocumentClient> GetDocumentClient(DocumentDBClientConfiguration documentDbConfigurations)
        {
            Contract.CheckValue(documentDbConfigurations, nameof(documentDbConfigurations));
            var key = string.Join("-", documentDbConfigurations.DatabaseUri, documentDbConfigurations.DatabaseId);
            if (!this.documentDBConnections.ContainsKey(key))
            {
                var databaseKey = await this.GetDatabaseSecret(documentDbConfigurations.DatabaseSecretName);
                await this.GetOrCreateDatabase(documentDbConfigurations.DatabaseUri, documentDbConfigurations.DatabaseId, databaseKey);
                this.documentDBConnections[key] = new KeyValuePair<string, string>(documentDbConfigurations.DatabaseUri, databaseKey);
            }

            var connection = this.documentDBConnections[key];
            return new DocumentClient(new Uri(connection.Key), connection.Value);
        }

        /// <summary>
        /// Get document client based on configuration
        /// </summary>
        /// <param name="documentDbStorageConfiguration">DB configurations</param>
        /// <returns>Document Client</returns>
        public async Task<DocumentClient> GetDocumentClient(DocumentDBStorageConfiguration documentDbStorageConfiguration)
        {
            Contract.CheckValue(documentDbStorageConfiguration, nameof(documentDbStorageConfiguration));

            var documentKey = string.Join("-", documentDbStorageConfiguration.KeyVaultUri, documentDbStorageConfiguration.ConnectionStringSecretName, documentDbStorageConfiguration.DatabaseId);
            if (!this.documentDBConnections.ContainsKey(documentKey))
            {
                var secretString = await this.GetDatabaseSecret(documentDbStorageConfiguration.ConnectionStringSecretName);
                var resourceAccessKey = DocumentDbConnectionStringParser.GetResourceAccessKey(secretString);
                var resourceEndpoint = DocumentDbConnectionStringParser.GetResourceEndpoint(secretString);
                await this.GetOrCreateDatabase(resourceEndpoint, documentDbStorageConfiguration.DatabaseId, resourceAccessKey);
                this.documentDBConnections[documentKey] = new KeyValuePair<string, string>(resourceEndpoint, resourceAccessKey);
            }

            var connection = this.documentDBConnections[documentKey];
            return new DocumentClient(new Uri(connection.Key), connection.Value);
        }

        /// <summary>
        /// Gets or creates document client
        /// </summary>
        /// <param name="databaseUri">Database URL</param>
        /// <param name="databaseId">Database Id</param>
        /// <param name="databaseKey">Database key</param>
        /// <returns>Document client</returns>
        private async Task GetOrCreateDatabase(string databaseUri, string databaseId, string databaseKey)
        {
            using (var client = new DocumentClient(new Uri(databaseUri), databaseKey))
            {
                await CommonLogger.Logger.ExecuteAsync(
                    "HcmReadDB",
                    async () =>
                    {
                        try
                        {
                            var response = await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseId));
                            this.logger.LogInformation($"Executed ReadDatabaseAsync on {databaseUri}: ActivityID {response?.ActivityId} ; Request Units: {response?.RequestCharge} ");
                        }
                        catch (DocumentClientException e)
                        {
                            var statusCode = (int)e.StatusCode;
                            if (e.StatusCode == HttpStatusCode.NotFound)
                            {
                                this.logger.LogInformation($"Document DB: Create databaseId: {databaseId}.");

                                await CommonLogger.Logger.ExecuteAsync(
                                    "HcmCreateDB",
                                    async () =>
                                    {
                                        var document = await client.CreateDatabaseAsync(new Database { Id = databaseId });
                                        this.logger.LogInformation($"Document DB: Executed CreateDatabaseAsync on {databaseId}: ActivityID {document?.ActivityId} ; Request Units: {document?.RequestCharge} ");
                                    });
                            }
                            else
                            {
                                throw new DocumentDBException($"Document DB: Create DB database id: {databaseId} failed with error: {e.Message}.");
                            }
                        }
                    });
            }
        }

        /// <summary>
        /// Gets the database secret from the key vault.
        /// </summary>
        /// <param name="keyVaultUri">The key vault URI</param>
        /// <param name="secretName">Name of key vault secret.</param>
        /// <returns>The database secret task.</returns>
        private async Task<string> GetDatabaseSecret(string secretName)
        {
            return await  this.secretManager.GetSecretAsync(secretName, this.logger);
        }
    }
}
