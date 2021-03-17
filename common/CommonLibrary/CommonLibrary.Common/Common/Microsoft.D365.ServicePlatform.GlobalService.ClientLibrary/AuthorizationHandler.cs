//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace CommonLibrary.ServicePlatform.GlobalService.ClientLibrary
{
    internal class AuthorizationHandler : DelegatingHandler
    {
        private readonly Func<Task<AuthenticationHeaderValue>> getAuthorizationHeaderAsync;

        public AuthorizationHandler(Func<Task<AuthenticationHeaderValue>> getAuthorizationHeaderAsync)
            : base()
        {
            this.getAuthorizationHeaderAsync = getAuthorizationHeaderAsync;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Authorization = await this.getAuthorizationHeaderAsync();
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
