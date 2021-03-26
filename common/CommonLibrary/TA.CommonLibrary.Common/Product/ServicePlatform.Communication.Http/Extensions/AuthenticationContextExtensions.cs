//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Context;
using TA.CommonLibrary.ServicePlatform.Communication.Http.Routers;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace TA.CommonLibrary.ServicePlatform.Communication.Http.Extensions
{
    public static class AuthenticationContextExtensions
    {
        public static async Task<string> AcquireTokenAsync(this AuthenticationContext context, ILoggerFactory loggerFactory, IHttpCommunicationClientFactory clientFactory, string userEmail, string password, string resourceId, string clientId)
        {
            return await AcquireTokenAsync(loggerFactory, clientFactory, context.Authority, userEmail, password, resourceId, clientId);
        }
        
        public static async Task<string> AcquireTokenAsync(ILoggerFactory loggerFactory, IHttpCommunicationClientFactory clientFactory, string authContextUri, string userEmail, string password, string resourceId, string clientId)
        {
            Contract.CheckValue(authContextUri, nameof(authContextUri));
            Contract.CheckValue(userEmail, nameof(userEmail));
            Contract.CheckValue(password, nameof(password));
            Contract.CheckValue(resourceId, nameof(resourceId));
            Contract.CheckValue(clientId, nameof(clientId));

            var logger = loggerFactory.CreateLogger("AuthenticationContextExtensions");
            
            return await logger.ExecuteAsync(
                "AcquireToken",
                async () =>
                {
                    logger.LogInformation($"Getting token for user {userEmail} to resource {resourceId} using client id {clientId}");

                    var uri = new Uri(authContextUri + @"/oauth2/token");
                    var str = uri.GetLeftPart(UriPartial.Authority);

                    using (var httpClient = clientFactory.CreateExternalApi(loggerFactory, new SingleUriRouter(new Uri(str))))
                    {
                        var bodyContent = new Dictionary<string, string>()
                        {
                            { "resource", resourceId },
                            { "client_id", clientId },
                            { "grant_type", "password" },
                            { "username", userEmail },
                            { "password", password },
                            { "scope", "openid" },
                        };
                        
                        var request = new HttpRequestMessage(HttpMethod.Post, new Uri(uri.PathAndQuery, UriKind.Relative))
                        {
                            Content = new FormUrlEncodedContent(bodyContent),
                        };

                        using (var response = await httpClient.SendAsync(request, default(CancellationToken)))
                        {
                            var token = await response.Content.ReadAsAsync<AuthenticationResultCore>();
                            return token.access_token;
                        }
                    }
                });
        }
        
        [Obsolete("Please use the method with the http factory and logger factory")]
        public static async Task<string> AcquireTokenAsync(this AuthenticationContext context, IHttpCommunicationClientFactory clientFactory, string authContextUri, string userEmail, string password, string resourceId, string clientId)
        {
            var uri = new Uri(authContextUri + @"/oauth2/token");
            var str = uri.GetLeftPart(UriPartial.Authority);

            using (var httpClient = clientFactory.CreateGTA(new SingleUriRouter(new Uri(str)), new HttpCommunicationClientOptions() { ThrowOnNonSuccessResponse = false }))
            {
                var bodyContent = new Dictionary<string, string>()
                {
                    { "resource", resourceId },
                    { "client_id", clientId },
                    { "grant_type", "password" },
                    { "username", userEmail },
                    { "password", password },
                    { "scope", "openid" },
                };
                
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(uri.PathAndQuery, UriKind.Relative))
                {
                    Content = new FormUrlEncodedContent(bodyContent),
                };

                using (var response = await httpClient.SendAsync(request, default(CancellationToken)))
                {
                    var token = await response.Content.ReadAsAsync<AuthenticationResultCore>();
                    return token.access_token;
                }
            }
        }

        [Obsolete("Please use the method with the http factory. Breaking change. Pass a new HttpCommunicationClientFactory()", true)]
        public static async Task<string> AcquireTokenAsync(this AuthenticationContext context, string authContextUri, string userEmail, string password, string resourceId, string clientId)
        {
            return null;
        }
    }

    public class AuthenticationResultCore
    {
        public string access_token { get; set; }
    }
}
