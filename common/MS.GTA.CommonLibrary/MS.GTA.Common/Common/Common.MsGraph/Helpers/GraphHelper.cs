//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Common.Common.Common.MsGraph.Helpers
{
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Common.Base.Configuration;
    using Common.MSGraph.Configuration;
    using ServicePlatform.Azure.Security;
    using System.Globalization;
    using System.Threading.Tasks;

    public static class GraphHelper
    {
        /// <summary>
        /// Gets the microsoft graph token.
        /// </summary>
        /// <param name="userAccessToken">The user access token.</param>
        /// <param name="aadClientConfig">The instnce of <see cref="AADClientConfiguration"/>.</param>
        /// <param name="graphConfig">The instnce of <see cref="MsGraphSetting"/>.</param>
        /// <param name="secretManager">The instnce for <see cref="ISecretManager"/>.</param>
        /// <param name="logger">The instnce for <see cref="ILogger"/>.</param>
        /// <returns>Access Token</returns>
        public static async Task<string> GetMicrosoftGraphToken(string userAccessToken, AADClientConfiguration aadClientConfig, MsGraphSetting graphConfig, ISecretManager secretManager, ILogger logger)
        {
            UserAssertion userAssertion = new UserAssertion(userAccessToken, "urn:ietf:params:oauth:grant-type:jwt-bearer");
            string aadInstance = aadClientConfig.AADInstance;
            string tenant = graphConfig.GraphTenant;
            string authority = string.Format(CultureInfo.InvariantCulture, aadInstance, tenant);
            AuthenticationContext authContext = new AuthenticationContext(authority);
            var clientId = aadClientConfig.ClientId;
            var clientSecret = await secretManager.GetSecretAsync(aadClientConfig.ClientCredential, logger);
            ClientCredential clientCredential = new ClientCredential(clientId, clientSecret);
            var result = await authContext.AcquireTokenAsync(graphConfig.GraphResourceId, clientCredential, userAssertion);
            return result.AccessToken;
        }
    }
}
