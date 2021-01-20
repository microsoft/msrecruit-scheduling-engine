// <copyright file="TokenDelegatingHandler.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.Product.ServicePlatform.AspNetCore.Http
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// The <see cref="TokenDelegatingHandler"/> class adds authorization header to the request.
    /// </summary>
    /// <seealso cref="DelegatingHandler" />
    internal class TokenDelegatingHandler : DelegatingHandler
    {
        /// <summary>
        /// The authorization header constant.
        /// </summary>
        private const string AuthorizationHeader = "Authorization";

        /// <summary>
        /// The HTTP context accessor.
        /// </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>
        /// The logger
        /// </summary>
        private readonly ILogger<TokenDelegatingHandler> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenDelegatingHandler"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException">
        /// httpContextAccessor
        /// or
        /// logger.
        /// </exception>
        public TokenDelegatingHandler(IHttpContextAccessor httpContextAccessor, ILogger<TokenDelegatingHandler> logger)
        {
            this.httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            string userToken;
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            if (!request.Headers.Contains(AuthorizationHeader))
            {
                userToken = this.GetUserToken();
                if (!string.IsNullOrWhiteSpace(userToken))
                {
                    request.Headers.Add(AuthorizationHeader, userToken);
                    response = await base.SendAsync(request, cancellationToken);
                }
            }

            return response;
        }

        private string GetUserToken()
        {
            HttpRequest request = this.httpContextAccessor.HttpContext.Request;
            var authorizationToken = request?.Headers?["Authorization"].Count > 0 ? request.Headers["Authorization"][0] : null;
            if (string.IsNullOrWhiteSpace(authorizationToken) || !authorizationToken.StartsWith("Bearer ", System.StringComparison.InvariantCultureIgnoreCase))
            {
                this.logger.LogError("There is no user token found on the request. No web notifications will be delivered.");
            }

            return authorizationToken;
        }
    }
}
