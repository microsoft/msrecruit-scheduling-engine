//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="MsGraphProvider.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------


namespace MS.GTA.Common.MSGraph
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web;
    using Base.Configuration;
    using Base.Exceptions;
    using Configuration;
    using Exceptions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Graph;
    using MS.GTA.Common.Base.Utilities;
    using MS.GTA.Common.Common.Common.MsGraph.Helpers;
    using MS.GTA.Common.MsGraph;
    using MS.GTA.Common.TestBase;
    using MS.GTA.ServicePlatform.Communication.Http;
    using MS.GTA.ServicePlatform.Communication.Http.Routers;
    using ServicePlatform.Azure.Security;
    using ServicePlatform.Communication.Http.Extensions;
    using ServicePlatform.Configuration;
    using ServicePlatform.Context;
    using ServicePlatform.Exceptions;

    using Constants = MsGraph.Constants;
    ////using Application = Contracts.Application;
    using Contract = CommonDataService.Common.Internal.Contract;
    using IAADClient = ServicePlatform.Azure.AAD.IAzureActiveDirectoryClient;
    using User = Microsoft.Graph.User;

    /// <summary>
    /// Microsoft Graph Provider class
    /// </summary>
    public class MsGraphProvider : IMsGraphProvider, IDisposable
    {
        /// <summary>
        /// Logger source instance
        /// </summary>
        private static ILogger<MsGraphProvider> logger;

        /// <summary>
        /// Graph service client instance
        /// </summary>
        private readonly GraphServiceClient graphClient;

        /// <summary>
        /// Azure active directory client instance
        /// </summary>
        private readonly AzureActiveDirectoryClient azureActiveDirectoryClient;

        /// <summary>
        /// Configuration manager instance
        /// </summary>
        private readonly IConfigurationManager configurationManager;

        private readonly IHttpCommunicationClientFactory httpClientFactory;

        private readonly IAADClient aadClient;

        /// <summary>
        /// AAD client configuration
        /// </summary>
        private readonly AADClientConfiguration aadClientConfig;

        /// <summary>
        /// Graph configuration
        /// </summary>
        private readonly MsGraphSetting graphConfig;

        /// <summary>
        /// secret Manager
        /// </summary>
        private readonly ISecretManager secretManager;

        private const string requestIdConst = "request-id";

        /// <summary>
        /// Initializes a new instance of the<see cref="MsGraphProvider" /> class.
        /// </summary>
        /// <param name="configurationManager">Configuration manager object</param>
        /// <param name="httpClientFactory">The HTTP client factory.</param>
        /// <param name="aadClient">aad client</param>
        /// <param name="logger">The logger.</param>
        /// <param name="secretManager">Secret manager.</param>
        /// <param name="memCacheName">Optional memory cache name</param>
        /// <param name="tenantId">The tenant Id. If passed the graph client will act in "App" mode, acquiring a token against the graph without a user context.</param>
        public MsGraphProvider(
            IConfigurationManager configurationManager,
            IHttpCommunicationClientFactory httpClientFactory,
            IAADClient aadClient,
            ILogger<MsGraphProvider> logger,
            ISecretManager secretManager,
            string memCacheName = null,
            string tenantId = null)
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager));

            this.graphConfig = configurationManager.Get<MsGraphSetting>();
            this.aadClientConfig = configurationManager.Get<AADClientConfiguration>();
            this.azureActiveDirectoryClient = new AzureActiveDirectoryClient(logger, this.aadClientConfig, this.graphConfig);
            this.configurationManager = configurationManager;
            this.httpClientFactory = httpClientFactory;
            this.aadClient = aadClient;

            this.graphClient = this.NewAuthenticatedClient(this.graphConfig.GraphBaseUrl);

            this.secretManager = secretManager;

            MsGraphProvider.logger = logger;
        }

        /// <summary>
        /// AADClient uses MemoryCache and clean up code can go here
        /// </summary>
        [Obsolete("Please use the user directory service client implementation for any future work on MSGraph. Going forward this will turn into an error.")]
        public void Dispose()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Acquire talent service authentication token
        /// </summary>
        /// <param name="userEmailPasswordSecret">Instance of User Email Password Secret</param>
        /// <param name="userSecretPassword">Email Password </param>
        /// <returns>The talent service token</returns>
        [Obsolete("Please use the user directory service client implementation for any future work on MSGraph, any questions/issues email vanguard@microsoft.com. Going forward this will turn into an error.")]
        public async Task<AuthenticationHeaderValue> GetGraphEmailResourceToken(UserEmailPasswordSecret userEmailPasswordSecret, string userSecretPassword = null)
        {
            var graphMailConfiguration = this.configurationManager.Get<GraphMailConfiguration>();
            var authConfig = this.configurationManager.Get<BearerTokenAuthentication>();
            var authContextUri = authConfig.Authority;
            var resourceId = authConfig.Audience;
            var clientId = graphMailConfiguration.NativeClientAppId; // Native AAD App Client Id to support logging in with username and password

            logger.LogInformation("Get graph resource token started");
            try
            {
                var token = await this.GetUserToken(userEmailPasswordSecret.UserEmail, userEmailPasswordSecret.PrimaryPassword, resourceId, clientId, authContextUri);
                
                //Removed resource Id from call as the target method was not using it.
                return await this.GetOnBehalfOfAccessToken(token);
            }
            catch (Exception e)
            {
                if (userSecretPassword == null)
                {
                    logger.LogError($"Get token failed with primary passwprd for email: {userEmailPasswordSecret.UserEmail},  Failed with Exception: {e}");
                    logger.LogInformation("Trying to get token with secondary password");
                    return await this.GetGraphEmailResourceToken(userEmailPasswordSecret, userEmailPasswordSecret.SecondaryPassword);
                }
                else
                {
                    throw new Exception($"Could not successfully generate token using any of the user name secrets");
                }
            }
        }

        /// <summary>
        /// Acquire talent service authentication token
        /// </summary>
        ///
        /// <returns>The talent service token</returns>
        [Obsolete("Please use the user directory service client implementation for any future work on MSGraph, any questions/issues email vanguard@microsoft.com. Going forward this will turn into an error.")]
        public async Task<AuthenticationHeaderValue> GetGraphSendEmailResourceToken(int maxRetries = 1)
        {
            var graphMailConfiguration = this.configurationManager.Get<GraphMailConfiguration>();
            var authConfig = this.configurationManager.Get<BearerTokenAuthentication>();
            var exceptions = new List<Exception>();

            var userNameSecrets = graphMailConfiguration.GraphEmailSecrets;
            var passwordSecrets = graphMailConfiguration.GraphEmailPasswordSecrets;

            var authContextUri = authConfig.Authority;
            var resourceId = authConfig.Audience;
            var clientId = graphMailConfiguration.NativeClientAppId; // Native AAD App Client Id to support logging in with username and password

            var usernameSecrets = new List<string>();
            var userpasswordSecrets = new List<string>();
            usernameSecrets.AddRange(userNameSecrets.Split(';').Select(t => t.Trim()));
            userpasswordSecrets.AddRange(passwordSecrets.Split(';').Select(t => t.Trim()));

            logger.LogInformation("Get graph resource token started");

            var retry = 0;
            while (true)
            {
                try
                {
                    for (var i = 0; i < userpasswordSecrets.Count(); i++)
                    {
                        var userNameSecret = usernameSecrets.Count() == 1 ? usernameSecrets[0] : usernameSecrets[i];
                        var passwordSecret = userpasswordSecrets[i];

                        try
                        {
                            var userEmail = await this.secretManager.ReadSecretAsync(userNameSecret);
                            var password = await this.secretManager.ReadSecretAsync(passwordSecret);
                            var token = await this.GetUserToken(userEmail.Value, password.Value, resourceId, clientId, authContextUri);
                            if (string.IsNullOrEmpty(token))
                            {
                                throw new GraphServiceException($"Unable to obtain a user token using {resourceId}");
                            }

                            var resourceToken = (await this.aadClient.GetOnBehalfOfAccessTokenAsync(this.graphConfig.GraphResourceId, token))?.AccessToken;
                            if (string.IsNullOrEmpty(resourceToken))
                            {
                                throw new GraphServiceException($"Unable to obtain a resource token using {this.graphConfig.GraphResourceId}");
                            }

                            logger.LogInformation("Graph resource token acquired successfully.");
                            return new AuthenticationHeaderValue(Base.Constants.BearerAuthenticationScheme, resourceToken);
                        }
                        catch (KeyVaultAccessException kve)
                        {
                            exceptions.Add(kve);
                        }
                        catch (AggregateException ae)
                        {
                            exceptions.Add(ae);
                        }
                        catch (Exception e)
                        {
                            exceptions.Add(e);
                        }
                    }

                    throw new AggregateException($"Could not successfully generate token using any of the usernamesecrets: {userNameSecrets} and password secrets: {passwordSecrets}", exceptions);
                }
                catch (Exception e)
                {
                    if (retry < maxRetries &&
                        e.InnerException is WebException &&
                        e.InnerException.GetHttpStatusCode() == (int)HttpStatusCode.ServiceUnavailable)
                    {
                        logger.LogWarning($"{nameof(GetGraphSendEmailResourceToken)} failed with Service unavailable error, retry count {retry}");
                        await Task.Delay(TimeSpan.FromSeconds(retry++));
                    }
                    else
                    {
                        logger.LogError($"Error: {e}");
                        throw new AcquireGraphTokenException(e);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the organization asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="maxRetries">The maximum retries.</param>
        /// <returns></returns>
        public async Task<IGraphServiceOrganizationCollectionPage> GetOrganizationAsync(string token, int maxRetries = 1)
        {
            if (string.IsNullOrEmpty(token))
            {
                return await this.CallWithRetry(token, maxRetries, nameof(GetOrganizationAsync), headerList => this.GetOrganizationInternal(headerList, true));
            }

            return await this.CallWithRetry(token, maxRetries, nameof(GetOrganizationAsync), headerList => this.GetOrganizationInternal(headerList, false));

        }

        /// <summary>
        /// Get users
        /// </summary>
        /// <param name="userId">User objectID</param>
        /// <param name="token">User access token</param>
        /// <param name="selectedFields">Select specific fields not returned by default. See <see cref="https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/user_get"/></param>
        /// <param name="maxRetries">The maximum number of retries.</param>
        /// <returns>User details collection</returns>
        [Obsolete("Please use the user directory service client implementation for any future work on MSGraph, any questions/issues email vanguard@microsoft.com. Going forward this will turn into an error.")]
        public async Task<User> GetUserAsync(string userId, string token, IEnumerable<Expression<Func<User, object>>> selectedFields = null, int maxRetries = 1)
        {
            if (string.IsNullOrEmpty(token))
            {
                return await this.CallWithRetry(token, maxRetries, nameof(GetUserAsync), headerList => this.GetUserInternal(userId, headerList, selectedFields, true));
            }

            return await this.CallWithRetry(token, maxRetries, nameof(GetUserAsync), headerList => this.GetUserInternal(userId, headerList, selectedFields, false));
        }

        /// <summary>
        /// Get users using email addresses
        /// </summary>
        /// <param name="emails">List of email addresses</param>
        /// <param name="token">User access token</param>
        /// <returns>List of User objects</returns>
        [Obsolete("Please use the user directory service client implementation for any future work on MSGraph, any questions/issues email vanguard@microsoft.com. Going forward this will turn into an error.")]
        public async Task<IEnumerable<User>> GetBulkUsersWithEmail(IEnumerable<string> emails, string token)
        {
            Contract.CheckValue(emails, nameof(emails));
            Contract.CheckValue(token, nameof(token));

            var result = emails.Select(async each =>
            {
                var response = await this.GetUsersWithPrefixAsync(each, token);
                var user = response?.ToList().FirstOrDefault();
                return user;
            });

            await Task.WhenAll(result);
            return result.Select(each => each.Result);
        }

        /// <summary>
        /// Get users starting with prefix
        /// </summary>
        /// <param name="prefix">Search prefix</param>
        /// <param name="token">User access token</param>
        /// <param name="maxRetries">The maximum number of retries.</param>
        /// <returns>User details collection</returns>
        [Obsolete("Please use the user directory service client implementation for any future work on MSGraph, any questions/issues email vanguard@microsoft.com. Going forward this will turn into an error.")]
        public async Task<IGraphServiceUsersCollectionPage> GetUsersWithPrefixAsync(string prefix, string token, int maxRetries = 1)
        {
            if (string.IsNullOrEmpty(token))
            {
                return await this.CallWithRetry(token, maxRetries, nameof(GetUsersWithPrefixAsync), headerList => this.GetUsersWithPrefixInternal(prefix, headerList, true));
            }

            return await this.CallWithRetry(token, maxRetries, nameof(GetUsersWithPrefixAsync), headerList => this.GetUsersWithPrefixInternal(prefix, headerList, false));

        }

        /// <summary>
        /// Get user photo
        /// </summary>
        /// <param name="userId">User objectID</param>
        /// <param name="token">User access token</param>
        /// <param name="maxRetries">The maximum number of retries.</param>
        /// <returns>Photo as base64 string</returns>
        public async Task<string> GetUserPhotoAsync(string userId, string token, int maxRetries = 1)
        {
            return await this.CallWithRetry(
                token,
                maxRetries,
                nameof(GetUserPhotoAsync),
                async headerList =>
                {
                    var imageStream = await this.GetUserPhotoInternal(userId, headerList);
                    return await this.ImageStreamToBase64Str(imageStream);
                });
        }

        /// <summary>
        /// Creates a new GraphService client
        /// </summary>
        /// <param name="graphBaseUrl">Graph URL along with version</param>
        /// <returns>Instance of graph service client</returns>
        public GraphServiceClient NewAuthenticatedClient(string graphBaseUrl)
        {
            var graphClient = new GraphServiceClient(
                graphBaseUrl,
                new DelegateAuthenticationProvider(this.CustomizeHeader));
            return graphClient;
        }

        /// <summary>
        /// Constructs header options list
        /// </summary>
        /// <param name="token">User access token</param>
        /// <param name="tokenExpired">Returns true if token has expired</param>
        /// <returns>List of header options</returns>
        protected static IList<HeaderOption> ConstructHeaderList(string token, bool tokenExpired)
        {
            logger.LogInformation($"Enter ConstructHeaderList with tokenExpired value {tokenExpired}");

            var headerOptions = new List<HeaderOption>
                {
                    new HeaderOption(
                        MsGraphConstants.TokenExpired,
                        tokenExpired.ToString()),
                    new HeaderOption(MsGraphConstants.Authorization, token),
                    new HeaderOption(requestIdConst, ServiceContext.Activity.Current?.RootActivityId.ToString()),
                };

            return headerOptions;
        }

        /// <summary>
        /// Creates Graph service client exception
        /// </summary>
        /// <param name="serviceException">Service Exception object</param>
        /// <returns>GraphService Exception</returns>
        protected static GraphServiceException CreateGraphServiceClientException(ServiceException serviceException)
        {
            var errorCodes = new List<string>();
            object requestId = null, date = null;

            for (var error = serviceException.Error; error != null; error = error.InnerError)
            {
                if (!string.IsNullOrEmpty(error.Code))
                {
                    errorCodes.Add(error.Code);
                }

                if (requestId == null && error.AdditionalData != null)
                {
                    error.AdditionalData.TryGetValue(requestIdConst, out requestId);
                }

                if (date == null && error.AdditionalData != null)
                {
                    error.AdditionalData.TryGetValue("date", out date);
                }
            }

            var traceMessage = $"GraphServiceException;Error={string.Join(",", errorCodes)}, Status Code: {serviceException.StatusCode} ,GraphServiceException={serviceException}, GraphErrorRequest-ID={requestId}, GraphErrorDate={date}";

            if (IsBenignGraphError(serviceException))
            {
                logger.LogInformation(traceMessage);
            }
            else
            {
                logger.LogError(traceMessage);
            }

            return new GraphServiceException(serviceException.Message, serviceException);
        }

        /// <summary>
        /// Populated request header
        /// </summary>
        /// <param name="headerOptions">Header options list</param>
        /// <param name="request">HTTP request message</param>
        /// <returns>Base Request encapsulates actual request object</returns>
        protected static IBaseRequest PopulateRequestHeader(IEnumerable<HeaderOption> headerOptions, IBaseRequest request)
        {
            logger.LogInformation("Populate request header for ms graph request");

            foreach (var headerOption in headerOptions)
            {
                request.Headers.Add(headerOption);
            }

            return request;
        }

        /// <summary>Get token </summary>
        /// <param name="userEmail">User email</param>
        /// <param name="password">user password</param>
        /// <param name="resourceId">The resource Id.</param>
        /// <param name="clientId">The client Id.</param>
        /// <param name="authContextUri">The Context URI.</param>
        /// <returns>User token</returns>
        private async Task<string> GetUserToken(string userEmail, string password, string resourceId, string clientId, string authContextUri)
        {
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            var token = await this.AcquireToken(authContextUri, userEmail, password, resourceId, clientId);

            if (string.IsNullOrWhiteSpace(token))
            {
                throw new GraphServiceException($"Unable to obtain a user token using {resourceId}");
            }

            logger.LogInformation("Acquired user token using user name and password.");
            return token;
        }

        private async Task<string> AcquireToken(string authContextUri, string userEmail, string password, string resourceId, string clientId)
        {
            var payload = $"resource={resourceId}&client_id={clientId}&grant_type=password&username={userEmail}&password={HttpUtility.UrlEncode(password)}&scope=openid";
            var uri = new Uri(authContextUri + @"/oauth2/token");
            var str = uri.GetLeftPart(UriPartial.Authority);

            using (var httpClient = this.httpClientFactory.CreateGTA(new SingleUriRouter(new Uri(str)), new HttpCommunicationClientOptions() { ThrowOnNonSuccessResponse = false }))
            {
                var request = new HttpRequestMessage(HttpMethod.Post, new Uri(uri.PathAndQuery, UriKind.Relative))
                {
                    Content = new StringContent(payload, Encoding.UTF8, "application/x-www-form-urlencoded")
                };

                using (var response = await httpClient.SendAsync(request, default(CancellationToken)))
                {
                    var token = await response.Content.ReadAsAsync<AuthenticationResultCore>();
                    return token.access_token;
                }
            }
        }

        /// <summary>
        /// Get on behalf of access email authentication token
        /// </summary>
        /// <param name="token">The access token</param>
        /// <returns>The talent service token</returns>
        private async Task<AuthenticationHeaderValue> GetOnBehalfOfAccessToken(string token)
        {
            var resourceToken = (await this.aadClient.GetOnBehalfOfAccessTokenAsync(this.graphConfig.GraphResourceId, token))?.AccessToken;

            if (string.IsNullOrWhiteSpace(resourceToken))
            {
                throw new GraphServiceException($"Unable to obtain a resource token using {this.graphConfig.GraphResourceId}");
            }

            logger.LogInformation("Graph resource token acquired successfully.");
            return new AuthenticationHeaderValue(Base.Constants.BearerAuthenticationScheme, resourceToken);
        }

        /// <summary>
        /// Make the specified graph call with logging and retry on authentication failure.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="token">The token.</param>
        /// <param name="maxRetries">The maximum number of retries.</param>
        /// <param name="callerFunctionName">The calling function name.</param>
        /// <param name="graphCallFunction">The graph call function.</param>
        /// <returns>The result.</returns>
        private async Task<TResult> CallWithRetry<TResult>(string token, int maxRetries, string callerFunctionName, Func<IList<HeaderOption>, Task<TResult>> graphCallFunction)
        {
            var retry = 0;
            while (true)
            {
                var headerList = ConstructHeaderList(token, retry != 0);

                try
                {
                    logger.LogInformation($"Starting graph call {callerFunctionName} (try {retry + 1} of {maxRetries}).");
                    var result = await graphCallFunction(headerList).ConfigureAwait(false);
                    logger.LogInformation($"Graph call {callerFunctionName} succeeded (try {retry + 1} of {maxRetries}).");
                    return result;
                }
                catch (ServiceException exception)
                {
                    if ((IsAuthenticationFailure(exception) || IsServiceUnavailableFailure(exception)) && retry < maxRetries)
                    {
                        logger.LogInformation($"Retry {callerFunctionName} on response failure (try {retry + 1} of {maxRetries}) after {retry} seconds.");
                        await Task.Delay(TimeSpan.FromSeconds(retry));
                        retry++;
                    }
                    else
                    {
                        if (IsServiceUnavailableFailure(exception))
                        {
                            logger.LogError($"Error: {exception}");
                            throw new MsGraphServiceUnavailableException(exception);
                        }
                        else if (exception.IsMatch(GraphErrorCode.InvalidRequest.ToString()) || String.Equals(exception?.Error?.Code, "Request_BadRequest"))
                        {
                            logger.LogError($"Error: {exception}");
                            throw new MsGraphInvalidInputException(exception);
                        }

                        throw CreateGraphServiceClientException(exception);
                    }
                }
            }
        }

        /// <summary>
        /// Get organization
        /// </summary>
        /// <param name="headerOptions">Header options</param>
        /// <param name="useCache">Specifies whether to use internal client or create a new one.</param>
        /// <returns>Organization page object.</returns>
        private async Task<IGraphServiceOrganizationCollectionPage> GetOrganizationInternal(IEnumerable<HeaderOption> headerOptions, bool useCache)
        {
            Contract.CheckValue(headerOptions, nameof(headerOptions));

            GraphServiceClient graphClient;

            if (useCache)
            {
                graphClient = this.graphClient;
            }
            else
            {
                graphClient = new GraphServiceClient(
                    this.graphConfig.GraphBaseUrl,
                    new DelegateAuthenticationProvider(this.CustomizeHeaderWithoutCaching));
            }

            var request = (GraphServiceOrganizationCollectionRequest)PopulateRequestHeader(headerOptions, graphClient.Organization.Request());

            logger.LogInformation($"Request send to Graph for GetManagerInternal: {request.Method} - {request.RequestUrl}");
            return await request.GetAsync();
        }

        private async Task CustomizeHeaderWithoutCaching(HttpRequestMessage request)
        {
            logger.LogInformation($"Adding access token on behalf of a user token for graph call.");
            var currentToken = request.Headers.GetValues(MsGraphConstants.Authorization);
            logger.LogInformation($"Fetched {currentToken?.Count()} tokens from the graph request");
            var result = await this.aadClient.GetOnBehalfOfAccessTokenAsync(this.graphConfig.GraphResourceId, currentToken.LastOrDefault());
            logger.LogInformation($"Fetched access token on behalf of user token for graph call");
            request.Headers.Authorization = new AuthenticationHeaderValue(Base.Constants.BearerAuthenticationScheme, result?.AccessToken);
        }

        /// <summary>
        /// Check authentication failure
        /// </summary>
        /// <param name="serviceException">Service Exception object</param>
        /// <returns>if it is authentication failure</returns>
        private static bool IsAuthenticationFailure(ServiceException serviceException)
        {
            logger.LogInformation($"Check Graph Error for authentication failure");

            return serviceException.IsMatch(Constants.InvalidAuthenticationToken)
                || serviceException.IsMatch(GraphErrorCode.AuthenticationFailure.ToString())
                || serviceException.IsMatch(Constants.AuthorizationIdentityNotFound)
                || serviceException.IsMatch(Constants.AuthenticationUnauthorized);
        }

        /// <summary>
        /// Check service unavailable failure
        /// </summary>
        /// <param name="serviceException">Service Exception object</param>
        /// <returns>if it is authentication failure</returns>
        private static bool IsServiceUnavailableFailure(ServiceException serviceException)
        {
            logger.LogInformation($"Check Service Error for service not available");

            return serviceException.IsMatch(GraphErrorCode.ServiceNotAvailable.ToString()) || serviceException.StatusCode == HttpStatusCode.ServiceUnavailable;
        }

        /// <summary>
        /// Creates Graph service client exception
        /// </summary>
        /// <param name="serviceException">Service Exception object</param>
        /// <returns>GraphService Exception</returns>
        private static bool IsBenignGraphError(ServiceException serviceException)
        {
            logger.LogInformation($"Check Graph Error for benign {serviceException}");
            return !(serviceException.IsMatch(GraphErrorCode.GeneralException.ToString())
                        || serviceException.IsMatch(GraphErrorCode.ChildItemCountExceeded.ToString())
                        || serviceException.IsMatch(GraphErrorCode.MalwareDetected.ToString())
                        || serviceException.IsMatch(GraphErrorCode.VirusSuspicious.ToString())
                        || serviceException.IsMatch(GraphErrorCode.Timeout.ToString())
                        || serviceException.IsMatch(GraphErrorCode.NotAllowed.ToString())
                        || serviceException.IsMatch(GraphErrorCode.ItemNotFound.ToString())
                        || serviceException.IsMatch(GraphErrorCode.NotSupported.ToString()));
        }

        /// <summary>
        /// Dispose managed and native code here
        /// </summary>
        /// <param name="clearManaged">If true clear managed code objects</param>
        private void Dispose(bool clearManaged)
        {
            this.azureActiveDirectoryClient?.Dispose();
        }

        /// <summary>
        /// Get users
        /// </summary>
        /// <param name="userPrincipalName">User principal name</param>
        /// <param name="headerOptions">Header options</param>
        /// <param name="selectedFields">Select specific fields not returned by default. See <see cref="https://developer.microsoft.com/en-us/graph/docs/api-reference/v1.0/api/user_get"/></param>
        /// <param name="useCache">Specifies whether to us the internal grap client or create a new one.</param>
        /// <returns>User details collection</returns>
        private Task<User> GetUserInternal(string userPrincipalName, IEnumerable<HeaderOption> headerOptions, IEnumerable<Expression<Func<User, object>>> selectedFields, bool useCache)
        {
            GraphServiceClient graphClient;

            if (useCache)
            {
                graphClient = this.graphClient;
            }
            else
            {
                graphClient = new GraphServiceClient(
                    this.graphConfig.GraphBaseUrl,
                    new DelegateAuthenticationProvider(this.CustomizeHeaderWithoutCaching));
            }

            var request = (IUserRequest)PopulateRequestHeader(headerOptions, graphClient.Users[userPrincipalName].Request());

            if (selectedFields != null)
            {
                foreach (var expr in selectedFields)
                {
                    request.Select(expr);
                }
            }

            logger.LogInformation($"Request send to Graph for GetUser: {request.Method} - {request.RequestUrl}");

            return request.GetAsync();
        }

        /// <summary>
        /// Get users starting with prefix
        /// </summary>
        /// <param name="prefix">Search prefix</param>
        /// <param name="headerOptions">Header options</param>
        /// <param name="useCache">Specifies whether to use the internal graph client or create a new one.</param>
        /// <returns>User details collection</returns>
        private Task<IGraphServiceUsersCollectionPage> GetUsersWithPrefixInternal(string prefix, IEnumerable<HeaderOption> headerOptions, bool useCache)
        {

            GraphServiceClient graphClient;

            if (useCache)
            {
                graphClient = this.graphClient;
            }
            else
            {
                graphClient = new GraphServiceClient(
                    this.graphConfig.GraphBaseUrl,
                    new DelegateAuthenticationProvider(this.CustomizeHeaderWithoutCaching));
            }

            var request = (IGraphServiceUsersCollectionRequest)PopulateRequestHeader(headerOptions, graphClient.Users.Request());

            request.Top(Constants.GraphSearchQueryLimit);

            if (prefix != null)
            {
                request.Filter($"startswith(displayName,'{prefix}') or startswith(userPrincipalName,'{prefix}') or startswith(mail,'{prefix}')");
            }

            logger.LogInformation($"Request send to Graph for GetUsers: {request.Method} - {request.RequestUrl}");
            return request.GetAsync();
        }

        /// <summary>
        /// Get user photo
        /// </summary>
        /// <param name="userId">User objectID</param>
        /// <param name="headerOptions">Header options</param>
        /// <returns>Photo stream</returns>
        private async Task<Stream> GetUserPhotoInternal(string userId, IEnumerable<HeaderOption> headerOptions)
        {
            var request = (IProfilePhotoContentRequest)PopulateRequestHeader(headerOptions, this.graphClient.Users[userId].Photo.Content.Request());

            logger.LogInformation($"Request send to Graph for GetUserPhotoInternal: {request.Method} - {request.RequestUrl}");

            return await request.GetAsync();
        }

        /// <summary>
        /// Convert image to base 64 string
        /// </summary>
        /// <param name="image">Image stream</param>
        /// <returns>Base64 image string</returns>
        private async Task<string> ImageStreamToBase64Str(Stream image)
        {
            logger.LogInformation("Start converting image stream to base64 string");

            var strBase64 = string.Empty;

            if (image != null)
            {
                byte[] bytedata;
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    bytedata = memoryStream.ToArray();
                }

                strBase64 = Convert.ToBase64String(bytedata);
            }

            return strBase64;
        }

        /// <summary>
        /// Gets on-behalf-of token from AADClient and sets request authorization header
        /// </summary>
        /// <param name="request">HTTP request message</param>
        /// <returns>Asynchronous task</returns>
        private async Task CustomizeHeader(HttpRequestMessage request)
        {
            var tokenExpiry = request.Headers.GetValues(MsGraphConstants.TokenExpired);
            var currentToken = request.Headers.GetValues(MsGraphConstants.Authorization);
            var cacheOption = !tokenExpiry.IsNullOrEmpty()
                    ? (object.Equals(tokenExpiry.LastOrDefault(), "True")
                    ? TokenCachingOptions.ForceRefreshCache
                    : TokenCachingOptions.PreferCache)
                    : TokenCachingOptions.ForceRefreshCache;

            var result = await GraphHelper.GetMicrosoftGraphToken(currentToken.LastOrDefault(), this.aadClientConfig, this.graphConfig, this.secretManager, logger);

            request.Headers.Authorization = new AuthenticationHeaderValue(Base.Constants.BearerAuthenticationScheme, result);
        }
    }
}
