//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Communication.Http;
using TA.CommonLibrary.ServicePlatform.Communication.Http.Routers;
using Microsoft.Extensions.Logging;

namespace TA.CommonLibrary.ServicePlatform.GlobalService.ClientLibrary
{
    /// <summary>
    /// A global service client which uses single uri routing.
    /// </summary>
    /// <seealso cref="TA.CommonLibrary.ServicePlatform.GlobalService.ClientLibrary.GlobalServiceClient" />
    public class ExternalGlobalServiceClient : GlobalServiceClient
    {
        /// <summary>
        /// Gets the router.
        /// </summary>
        protected override IHttpRouter Router { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalGlobalServiceClient"/> class.
        /// </summary>
        /// <param name="factory">The http communication client factory.</param>
        /// <param name="getAuthorizationHeaderAsync">The function to get an authorization header asynchronously.</param>
        /// <param name="clusterUri">The Uri of the cluster in which the target global service application is hosted.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="applicationName">Optional name of the Global Service application. Defaults to GlobalService.</param>
        public ExternalGlobalServiceClient(IHttpCommunicationClientFactory factory, Func<Task<AuthenticationHeaderValue>> getAuthorizationHeaderAsync, Uri clusterUri, ILogger logger, string applicationName = DefaultApplicationName)
            : this(factory, getAuthorizationHeaderAsync, new Uri(clusterUri, $"{applicationName}/{ServiceName}"), logger)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalGlobalServiceClient"/> class.
        /// </summary>
        /// <param name="factory">The http communication client factory.</param>
        /// <param name="getAuthorizationHeaderAsync">The function to get an authorization header asynchronously.</param>
        /// <param name="globalServiceUri">The Uri to the GlobalService.</param>
        /// <param name="logger">The logger.</param>
        public ExternalGlobalServiceClient(IHttpCommunicationClientFactory factory, Func<Task<AuthenticationHeaderValue>> getAuthorizationHeaderAsync, Uri globalServiceUri, ILogger logger)
            : base(factory, getAuthorizationHeaderAsync, logger)
        {
            Contract.CheckValue(globalServiceUri, nameof(globalServiceUri));

            this.Router = new SingleUriRouter(globalServiceUri);
        }
    }
}
