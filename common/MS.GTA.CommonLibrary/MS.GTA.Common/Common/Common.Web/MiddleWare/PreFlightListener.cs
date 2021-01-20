//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="PreFlightListener.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Web.MiddleWare
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Cors.Infrastructure;
    using Microsoft.AspNetCore.Http;
    using Base.Utilities;
    using ServicePlatform.Context;

    /// <summary>
    /// This class handles pre flights of application. 
    /// </summary>
    public class PreFlightListener
    {
        /// <summary>
        /// Access control header constant
        /// </summary>
        private const string AccessControlheader = "Access-Control-Allow-Origin";

        /// <summary>
        /// Request delegate
        /// </summary>
        private readonly RequestDelegate request;

        /// <summary>
        /// Header dictionary
        /// </summary>
        private readonly IHeaderDictionary headerDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="PreFlightListener" /> class.
        /// </summary>
        /// <param name="request">Request delegate instance</param>
        /// <param name="headerDictionary">Header dictionary instance</param>
        public PreFlightListener(RequestDelegate request, IHeaderDictionary headerDictionary)
        {
            this.request = request;
            this.headerDictionary = headerDictionary;
        }

        /// <summary>
        /// Takes in HTTP context and adds response headers
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns>Awaits till invoke operation is complete</returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Headers.ContainsKey(CorsConstants.Origin))
            {
                if (string.Equals(context.Request.Method, CorsConstants.PreflightHttpMethod, StringComparison.OrdinalIgnoreCase))
                {
                    CommonLogger.Logger.Execute(
                        "HcmOptionsVerb",
                        () =>
                        {
                            foreach (var header in this.headerDictionary)
                            {
                                context.Response.Headers.Add(header.Key, header.Value);
                            }

                            context.Response.StatusCode = StatusCodes.Status200OK;
                        });

                    return;
                }
                else
                {
                    if (this.headerDictionary.Keys.Contains(AccessControlheader))
                    {
                        context.Response.Headers.Add(AccessControlheader, this.headerDictionary[AccessControlheader]);
                    }
                }
            }

            await this.request(context);
        }
    }
}
