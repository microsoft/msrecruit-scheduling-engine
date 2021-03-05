//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TestBase.Utils
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Common.Base.Configuration;
    using Common.Base.Utilities;
    using Common.Routing.Configuration;
    using Common.Routing.Constants;
    using Common.Routing.GlobalService;
    using ServicePlatform.Azure.AAD;
    using ServicePlatform.Communication.Http;
    using ServicePlatform.Configuration;
    using ServicePlatform.GlobalService.ClientLibrary;
    using ServicePlatform.Tracing;
    using ServicePlatform.Security;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;

    /// <summary>
    /// Extraction of TenantId from token
    /// </summary>
    public static class TokenExtraction
    {
        /// <summary>
        /// Gets the cluster uri based up on tenantid
        /// </summary>
        /// <param name="configurationManager">The configuration Manager</param>
        /// <param name="loggerFactory">The instnce for <see cref="ILoggerFactory"/>.</param>
        /// <param name="environmentId">The Environment ID</param>
        /// <param name="environmentName">The environment name</param>
        /// <param name="trace">The trace</param>
        /// <returns>The cluster Uri</returns>
        public static async Task<string> GetClusterUri(IConfigurationManager configurationManager, ILoggerFactory loggerFactory, string environmentId, string environmentName, ITraceSource trace)
        {
            var httpServiceClientFactory = new HttpCommunicationClientFactory(loggerFactory);

            var globalServiceConfiguration = GlobalServiceEnvironmentSettings.GetGlobalServiceConfiguration(environmentName);
            var globalServiceClient = CreateGlobalServiceClient(loggerFactory.CreateLogger<ExternalGlobalServiceClient>(), httpServiceClientFactory, globalServiceConfiguration, configurationManager);

            var handler = new JwtSecurityTokenHandler();
            var clusterUri = await globalServiceClient.GetEnvironmentCnameAsync(environmentId);

            return clusterUri;
        }

        /// <summary>
        /// Creates the singleton external global service client
        /// </summary>
        /// <param name="logger">The Logger</param>
        /// <param name="communicationFactory">The http  communication client factory</param>
        /// <param name="globalServiceConfig">The global service configuration</param>
        /// <param name="configManager">The configuration Manager</param>
        /// <returns>The <see cref="IGlobalServiceClient"/></returns>
        private static IGlobalServiceClient CreateGlobalServiceClient(ILogger logger, IHttpCommunicationClientFactory communicationFactory, GlobalServiceConfiguration globalServiceConfig, IConfigurationManager configManager)
        {
            Func<Task<AuthenticationHeaderValue>> tokenRetriever = async () =>
            {
                var aadClientConfig = configManager.Get<AADClientConfiguration>();
                var exceptions = new List<Exception>();
                var certificateManager = new CertificateManager();
                var thumprints = aadClientConfig.ClientCertificateThumbprintList;
                foreach (var thumprint in thumprints)
                {
                    try
                    {
                        var authority = aadClientConfig.AADInstance.FormatWithInvariantCulture(aadClientConfig.TenantID);
                        var authContext = new AuthenticationContext(authority);
                        var servicePlatformAAdConfig = configManager.Get<AzureActiveDirectoryClientConfiguration>();
                        var clientCertificate = certificateManager.FindByThumbprint(thumprint, servicePlatformAAdConfig.ClientCertificateStoreName, servicePlatformAAdConfig.ClientCertificateStoreLocation);
                        var clientAssertion = new ClientAssertionCertificate(aadClientConfig.ClientId, clientCertificate);

                        var authenticationResult = await authContext.AcquireTokenAsync(globalServiceConfig.Resource, clientAssertion);
                        return new AuthenticationHeaderValue(authenticationResult.AccessTokenType, authenticationResult.AccessToken);
                    }
                    catch (Exception ex)
                    {
                        exceptions.Add(ex);
                    }
                }

                throw new AggregateException($"Could not successfully acquire certificate using thumbprints: {string.Join(", ", aadClientConfig.ClientCertificateThumbprintList)}", exceptions);
            };

            var uri = new Uri(globalServiceConfig.ClusterUri);
            return new ExternalGlobalServiceClient(
                communicationFactory,
                tokenRetriever,
                uri,
                logger);
        }
    }
}
