//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.AspNetCore.Http.Abstractions;
using TA.CommonLibrary.ServicePlatform.Exceptions;
using System.Text.RegularExpressions;

namespace TA.CommonLibrary.ServicePlatform.AspNetCore.Http
{
    /// <summary>
    /// Service Fabric Routing
    /// </summary>
    internal sealed class ServiceFabricRoutingMiddleware : IStatelessHttpMiddleware
    {
        private readonly IList<string> serverAddresses;

        public ServiceFabricRoutingMiddleware(IList<string> serverAddresses)
        {
            Contract.CheckAllNonEmpty(serverAddresses, nameof(serverAddresses));
            this.serverAddresses = new List<string>();

            // This regex will match for urls likes "https://+:443/"
            Regex wildUrlRegex = new Regex(@"^((http[s]?):\/\/)(\+:[0-9]{2,5})((\/){1}[\/\w]*)?$");

            foreach (string address in serverAddresses)
            {
                if(string.IsNullOrEmpty(address))
                {
                    continue;
                }

                Uri uri = null;
                if (Uri.TryCreate(address, UriKind.RelativeOrAbsolute, out uri))
                {
                    this.serverAddresses.Add(uri.AbsolutePath);
                }
                else
                {
                    var match = wildUrlRegex.Match(address.ToLower());
                    if(match.Success)
                    {
                        this.serverAddresses.Add(match.Groups[4].Value);
                    }
                    // TODO if match fails then should we throw?
                }
            }            
        }

        public async Task InvokeAsync(HttpContext httpContext, Func<Task> next)
        {
            Contract.AssertValue(httpContext, nameof(httpContext));
            Contract.AssertValue(next, nameof(next));

            // If request is for another service but happened to be received by this service return 410 response immediately to force re-resolve from gateway.
            var requestUri = new Uri(httpContext.Request.GetDisplayUrl());
            var requestIsForThisService = serverAddresses.Any(address => requestUri.AbsolutePath.StartsWith(address, StringComparison.OrdinalIgnoreCase));

            if (!requestIsForThisService)
            {
                throw new RequestIntendedForOtherServiceException(requestUri.LocalPath, string.Join(", ", serverAddresses)).EnsureTraced();
            }

            const string serviceFabricHeaderName = "X-ServiceFabric";
            const string serviceFabricHeaderValue = "ResourceNotFound";

            httpContext.Response.OnStarting(() =>
            {
                // Adds X-ServiceFabric: ResourceNotFound header to outbound 404 responses to indicate to the SF Application Gateway(Reverse Proxy) that
                // it should NOT attempt to re-resolve this service.
                if (httpContext.Response.StatusCode == (int)HttpStatusCode.NotFound && !httpContext.Response.Headers.ContainsKey(serviceFabricHeaderName))
                {
                    httpContext.Response.Headers.Add(serviceFabricHeaderName, serviceFabricHeaderValue);
                }

                return Task.FromResult(0);
            });

            await next();
        }
    }
}
