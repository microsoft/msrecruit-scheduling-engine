//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.Wopi.Discovery
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Sockets;
    using System.Runtime.Caching;
    using System.Threading.Tasks;
    using System.Xml.Linq;
    using MS.GTA.Common.Base.Utilities;
    using CommonDataService.Common.Internal;
    using MS.GTA.ServicePlatform.Context;
    using MS.GTA.ServicePlatform.Exceptions;
    using MS.GTA.ServicePlatform.Tracing;
    using Exceptions;
    using Interfaces;
    using MS.GTA.Common.Web.Utils;

    /// <summary>
    /// The WOPIDiscovery class
    /// </summary>
    public class WopiDiscovery : IWopiDiscovery, IDisposable
    {
        /// <summary>
        /// Relative path to discovery information on WOPI Clients
        /// </summary>
        private const string DiscoveryPath = "/hosting/discovery";

        /// <summary>
        /// WOPI Discovery XML element named app
        /// </summary>
        private const string WopiElementApp = "app";

        /// <summary>
        /// WOPI Discovery XML element named action
        /// </summary>
        private const string WopiElementAction = "action";

        /// <summary>
        /// WOPI Discovery XML element named proof-key
        /// </summary>
        private const string WopiElementProofKey = "proof-key";

        /// <summary>
        /// WOPI Discovery XML attribute named name
        /// </summary>
        private const string WopiAttributeName = "name";

        /// <summary>
        /// WOPI Discovery XML action attribute named URLSRC
        /// </summary>
        private const string WopiActionAttributeUrlsrc = "urlsrc";

        /// <summary>
        /// WOPI Discovery XML action attribute named ext
        /// </summary>
        private const string WopiActionAttributeExt = "ext";

        /// <summary>
        /// WOPI Discovery XML app attribute named FAVIconUrl
        /// </summary>
        private const string WopiAppAttributeFavIcon = "favIconUrl";

        /// <summary>
        /// WOPI proof key attributes
        /// </summary>
        private const string WopiProofKeyAttributeOldValue = "oldvalue";

        /// <summary>
        /// WOPI proof key attributes
        /// </summary>
        private const string WopiProofKeyAttributeValue = "value";

        /// <summary>
        /// Time window to cache discovery information for in minutes
        /// </summary>
        private const double CacheHoldTime = 10.0;

        /// <summary>
        /// Time window before attempting to retrieve discovery information again; in minutes
        /// </summary>
        private const double RetryTime = 5.0;

        /// <summary>
        /// retry count on failure
        /// </summary>
        private const int RetryCount = 3;

        /// <summary>
        /// Supported WOPI applications
        /// </summary>
        private readonly string[] supportedWopiApps = Enum.GetNames(typeof(WopiApps));

        /// <summary>
        /// Supported WOPI app actions
        /// </summary>
        private readonly string[] supportedWopiActions = Enum.GetNames(typeof(WopiActions));

        /// <summary>
        /// Trace source
        /// </summary>
        private readonly ITraceSource trace;

        /// <summary>
        ///  To detect redundant calls
        /// </summary>
        private bool disposedValue = false;

        /// <summary>
        /// The memory cache for holding discovery information
        /// </summary>
        private MemoryCache cache = new MemoryCache("cache");

        private readonly HttpClient httpClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiDiscovery" /> class.
        /// </summary>
        /// <param name="trace">Trace source instance</param>
        public WopiDiscovery(ITraceSource trace)
        {
            Contract.CheckValue(trace, "trace should be provided");

            this.trace = trace;
            var retryHelper = new HttpRetryMessageHandler(new HttpClientHandler(), this.trace);
            this.httpClient = new HttpClient(retryHelper);
        }

        /// <summary>
        /// Gets the WOPI application favicon
        /// </summary>
        /// <param name="wopiClientUrl">The WOPI client to look up</param>
        /// <param name="extension">The extension to look up</param>
        /// <param name="action">The action name</param>
        /// <returns>A string representing the favicon URI</returns>
        public async Task<string> GetApplicationFavIconUrlAsync(string wopiClientUrl, string extension, WopiActions action)
        {
            Contract.CheckNonEmpty(wopiClientUrl, nameof(wopiClientUrl), "wopiClientUrl should not be null!");
            Contract.CheckNonEmpty(extension, nameof(extension), "extension should not be null!");

            this.trace.TraceInformation($"WopiDiscovery: Retrieving FavIcon for {wopiClientUrl} with action: {action} and extension: {extension}");
            var info = await this.FindWopiActionInfoAsync(wopiClientUrl, extension, action);

            if (info != null)
            {
                return info.FavIconUrl;
            }

            return null;
        }

        /// <summary>
        /// Gets the WOPI client's proof information
        /// </summary>
        /// <param name="wopiClientUrl">The WOPI client to look up</param>
        /// <returns>The WOPI proof information for the WOPI client</returns>
        public async Task<WopiProofInfo> GetProofInformationAsync(string wopiClientUrl)
        {
            Contract.CheckNonEmpty(wopiClientUrl, nameof(wopiClientUrl), "wopiClientUrl should not be null!");

            this.trace.TraceInformation($"WopiDiscovery: Retrieving proof information for {wopiClientUrl}");
            var cachedValues = await this.GetCachedCollectionAsync(wopiClientUrl.ToUpperInvariant());
            if (cachedValues != null)
            {
                return cachedValues.ProofInfo;
            }

            return null;
        }

        /// <summary>
        /// Gets the viewing URL that browsers can use to render the file
        /// </summary>
        /// <param name="wopiClientUrl">The WOPI client to look up</param>
        /// <param name="extension">The extension to look up</param>
        /// <param name="action">The action name</param>
        /// <returns>A string representing the render URI</returns>
        public async Task<string> GetViewingURLTemplateAsync(string wopiClientUrl, string extension, WopiActions action)
        {
            Contract.CheckNonEmpty(wopiClientUrl, nameof(wopiClientUrl), "wopiClientUrl should not be null!");
            Contract.CheckNonEmpty(extension, nameof(extension), "extension should not be null!");

            this.trace.TraceInformation($"WopiDiscovery: Retrieving viewing URL template for {wopiClientUrl} with action: {action} and extension: {extension}");
            var info = await this.FindWopiActionInfoAsync(wopiClientUrl, extension, action);

            if (info != null)
            {
                return info.Url;
            }

            return null;
        }

        /// <summary>
        /// Checks if an action is valid for an extension type
        /// </summary>
        /// <param name="wopiClientUrl">The WOPI client to look up</param>
        /// <param name="extension">The extension to look up</param>
        /// <param name="action">The action name</param>
        /// <returns>True if the event is allowed</returns>
        public async Task<bool> CanDoActionAsync(string wopiClientUrl, string extension, WopiActions action)
        {
            Contract.CheckNonEmpty(wopiClientUrl, nameof(wopiClientUrl), "wopiClientUrl should not be null!");
            Contract.CheckNonEmpty(extension, nameof(extension), "extension should not be null!");

            this.trace.TraceInformation($"WopiDiscovery: Checking if wopiClient @ {wopiClientUrl} suports action: {action} on extension type: {extension}");
            var info = await this.FindWopiActionInfoAsync(wopiClientUrl, extension, action);
            return info != null;
        }

        /// <summary>
        /// Dispose implementation
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose state handler
        /// </summary>
        /// <param name="disposing">disposing state</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.cache.Dispose();
                }

                this.disposedValue = true;
            }
        }

        /// <summary>
        /// Find the WOPI action information for an extension and action pair
        /// </summary>
        /// <param name="wopiClientUrl">The WOPI client to look up</param>
        /// <param name="extension">The extension to look up</param>
        /// <param name="action">The desired action to carry out on the extension type</param>
        /// <returns>The WOPI Action information object that matches or null if not found</returns>
        private async Task<WopiActionInfo> FindWopiActionInfoAsync(string wopiClientUrl, string extension, WopiActions action)
        {
            Contract.CheckNonEmpty(wopiClientUrl, nameof(wopiClientUrl), "wopiClientUrl should not be null!");
            Contract.CheckNonEmpty(extension, nameof(extension), "extension should not be null!");

            WopiActionInfo info = null;
            var cachedValues = await this.GetCachedCollectionAsync(wopiClientUrl.ToUpperInvariant());

            if (cachedValues != null)
            {
                info = cachedValues.ActionInfo.FirstOrDefault(cv =>
                    cv.Extension == extension.ToLowerInvariant() &&
                    cv.Action == action.ToString().ToLowerInvariant());
            }

            return info;
        }

        /// <summary>
        /// Gets the discovery URI for a WOPI Client
        /// </summary>
        /// <param name="wopiClientUrl">The WOPI client URI</param>
        /// <returns>Path to the discovery file on the WOPI client</returns>
        private Uri GetDiscoveryURL(string wopiClientUrl)
        {
            Contract.CheckNonEmpty(wopiClientUrl, nameof(wopiClientUrl), "wopiClientUrl should not be null!");

            return new Uri(new Uri(wopiClientUrl), DiscoveryPath);
        }

        /// <summary>
        /// Gets the cached WOPI discovery information for a WOPI Client
        /// </summary>
        /// <param name="wopiClientUrl">The client to retrieve the cached information for</param>
        /// <returns>A collection of WOPIDiscoveryInfo objects</returns>
        private async Task<WopiDiscoveryInfo> GetCachedCollectionAsync(string wopiClientUrl)
        {
            Contract.CheckNonEmpty(wopiClientUrl, nameof(wopiClientUrl), "wopiClientUrl should not be null!");
            WopiDiscoveryInfo cachedValues = null;

            try
            {
                var cachedObject = this.cache.Get(wopiClientUrl);
                if (cachedObject != null)
                {
                    this.trace.TraceInformation($"Reusing cached data");
                    cachedValues = (WopiDiscoveryInfo)cachedObject;
                }
                else
                {
                    this.trace.TraceInformation($"Null cached value; attempting to load discovery information from wopiClient at {wopiClientUrl}");
                    cachedValues = await this.LoadDiscoveryXmlAsync(wopiClientUrl);
                    var item = new CacheItem(wopiClientUrl, cachedValues);
                    this.cache.Set(item, this.InitializeCachePolicy(CacheHoldTime));
                }
            }
            catch
            {
                // We should cache a not found value here when the wopi discovery url can't be hit
                // otherwise each request to the cache will initiate a call and provide a bad user experince to the user.
                // We will have a separate cache expiration policy for this since we expect the server to not remain down for long.
                this.trace.TraceInformation($"WopiDiscovery: Wopi Client @ {wopiClientUrl} not reachable.");
                CacheItem item = new CacheItem(wopiClientUrl, new WopiDiscoveryInfo());
                this.cache.Set(item, this.InitializeCachePolicy(RetryTime));
            }

            return cachedValues;
        }

        /// <summary>
        /// Loads the discovery information from the WOPI Client
        /// </summary>
        /// <param name="wopiClientUrl">The url of the WOPI Client</param>
        /// <param name="retryCount">Retry Count</param>
        /// <returns>A collection of WOPIDiscoveryInfo objects</returns>
        private async Task<WopiDiscoveryInfo> LoadDiscoveryXmlAsync(string wopiClientUrl, int retryCount = 0)
        {
            Contract.CheckNonEmpty(wopiClientUrl, nameof(wopiClientUrl), "wopiClientUrl should not be null!");

            var url = this.GetDiscoveryURL(wopiClientUrl);

            return await CommonLogger.Logger.ExecuteAsync(
                "HcmCmnWopiDscvry",
                async () =>
                {
                    try
                    {
                        using (var response = await this.httpClient.GetAsync(url))
                        {
                            response.EnsureSuccessStatusCode();

                            using (Stream stream = await response.Content.ReadAsStreamAsync())
                            {
                                using (StreamReader reader = new StreamReader(stream))
                                {
                                    return this.Parse(XDocument.Parse(reader.ReadToEnd()));
                                }
                            }
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        if (retryCount < RetryCount)
                        {
                            while (ex.InnerException != null)
                            {
                                if (ex.InnerException is SocketException)
                                {
                                    await this.LoadDiscoveryXmlAsync(wopiClientUrl, ++retryCount);
                                }
                            }
                        }
                        
                        throw new WopiClientDiscoveryException(ex.ToString()).EnsureTraced(trace);
                    }
                    catch (Exception e)
                    {
                        throw new WopiClientDiscoveryException(e.ToString()).EnsureTraced(trace);
                    }
                });
        }

        /// <summary>
        /// Parses the discovery XML doc and extracts needed information
        /// </summary>
        /// <param name="discoveryXml">The XML document representing the discovery from the WOPI Client</param>
        /// <returns>A collection of WOPIDiscoveryInfo objects</returns>
        private WopiDiscoveryInfo Parse(XDocument discoveryXml)
        {
            Contract.CheckValue(discoveryXml, "discoveryXml should not be null!");

            var wopiProofInfo = discoveryXml.Descendants(WopiElementProofKey)
                .Select(x =>
                    new WopiProofInfo()
                    {
                        CurrentCspBlob = x.Attribute(WopiProofKeyAttributeValue).Value,
                        OldCspBlob = x.Attribute(WopiProofKeyAttributeOldValue).Value
                    })
                .First();

            var wopiActionInfoList = discoveryXml
                .Descendants(WopiElementApp)
                .Where(a => this.supportedWopiApps.Contains(a.Attribute(WopiAttributeName).Value, StringComparer.OrdinalIgnoreCase))
                .Descendants(WopiElementAction)
                .Where(a => this.supportedWopiActions.Contains(a.Attribute(WopiAttributeName).Value, StringComparer.OrdinalIgnoreCase))
                .Select(x =>
                    new WopiActionInfo()
                    {
                        FavIconUrl = x.Parent.Attribute(WopiAppAttributeFavIcon).Value,
                        Url = x.Attribute(WopiActionAttributeUrlsrc).Value,
                        Extension = x.Attribute(WopiActionAttributeExt).Value.ToLowerInvariant(),
                        Action = x.Attribute(WopiAttributeName).Value.ToLowerInvariant()
                    })
                .ToArray();

            return new WopiDiscoveryInfo()
            {
                ActionInfo = wopiActionInfoList,
                ProofInfo = wopiProofInfo
            };
        }

        /// <summary>
        /// Initializes the cache policy
        /// </summary>
        /// <param name="minutesToCache">Lifetime of the cache</param>
        /// <returns>A cache item policy</returns>
        private CacheItemPolicy InitializeCachePolicy(double minutesToCache)
        {
            Contract.CheckParam(minutesToCache >= 0, "minutesToCache should be >= 0!");

            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(minutesToCache);
            return policy;
        }
    }
}
