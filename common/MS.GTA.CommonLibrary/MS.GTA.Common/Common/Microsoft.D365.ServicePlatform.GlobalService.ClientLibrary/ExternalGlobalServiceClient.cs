using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Communication.Http;
using MS.GTA.ServicePlatform.Communication.Http.Routers;
using Microsoft.Extensions.Logging;

namespace MS.GTA.ServicePlatform.GlobalService.ClientLibrary
{
    /// <summary>
    /// A global service client which uses single uri routing.
    /// </summary>
    /// <seealso cref="MS.GTA.ServicePlatform.GlobalService.ClientLibrary.GlobalServiceClient" />
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
