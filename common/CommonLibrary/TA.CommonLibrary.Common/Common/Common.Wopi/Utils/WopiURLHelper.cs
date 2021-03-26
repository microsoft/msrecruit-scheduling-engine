//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Wopi.Utils
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using CommonDataService.Common.Internal;
    using Configuration;
    using TA.CommonLibrary.ServicePlatform.Configuration;
    using TA.CommonLibrary.ServicePlatform.Tracing;
    using Discovery;
    using Interfaces;

    /// <summary>
    /// The WOPI URL helper class
    /// </summary>
    public class WopiURLHelper : IWopiURLHelper
    {
        /// <summary>Trace source</summary>
        private readonly ITraceSource trace;

        /// <summary>
        /// Initializes a new instance of the <see cref="WopiURLHelper"/> class.
        /// </summary>
        /// <param name="discovery">The Discovery instance for retrieving uris</param>
        /// <param name="configurationManager">Configuration settings</param>
        /// <param name="trace">Trace source instance</param>
        public WopiURLHelper(IWopiDiscovery discovery, IConfigurationManager configurationManager, ITraceSource trace)
        {
            Contract.CheckValue(discovery, "Discovery instance should not be null!");
            Contract.CheckValue(configurationManager, "configurationManager should not be null!");
            Contract.CheckValue(trace, nameof(trace), "trace should be provided");
            var configSettings = configurationManager.Get<WopiSetting>();

            this.WopiDiscovery = discovery;
            this.WopiServerURL = configSettings.WopiServerUrl;
            this.WopiClientURL = configSettings.WopiClientUrl;
            this.trace = trace;
        }

        /// <summary>
        /// Gets the URI of the WOPI server; our controller providing access to files for WOPI
        /// </summary>
        public string WopiServerURL { get; }

        /// <summary>
        /// Gets the URI of the WOPI Client
        /// </summary>
        public string WopiClientURL { get; }

        /// <summary>
        /// Gets the WOPIDiscovery instance
        /// </summary>
        public IWopiDiscovery WopiDiscovery { get; }

        /// <summary>
        /// Gets the full viewing URL to be sent to a browser for viewing a file
        /// </summary>
        /// <param name="extension">The extension to look up</param>
        /// <param name="action">The desired action to carry out on the extension type</param>
        /// <param name="fileId">The file to carry out the action on</param>
        /// <returns>The URL to be sent to browser clients for carrying out the action on the file</returns>
        public async Task<string> GetViewingURLAsync(
            string extension,
            WopiActions action,
            string fileId)
        {
            Contract.CheckNonEmpty(extension, nameof(extension), "extension should not be null!");
            Contract.CheckNonEmpty(fileId, nameof(fileId), "fileId should not be null!");

            var urlTemplate = await this.WopiDiscovery.GetViewingURLTemplateAsync(this.WopiClientURL, extension, action);

            if (urlTemplate == null)
            {
                this.trace.TraceInformation($"WopiURLHelper: URLTemplate from WOPI Client: ${this.WopiClientURL} is null; returning");
                return null;
            }

            this.trace.TraceInformation($"WopiURLHelper: WOPI Server URL: ${this.WopiServerURL}");
            var baseUri = this.WopiServerURL.TrimEnd('/');
            var fileURI = $"{baseUri}/v1/wopi/files/{fileId}";
            var queryParams = new Dictionary<string, string>()
            {
                { "WOPISrc", fileURI }
            };
            var queryString = this.CreateQueryString(queryParams);

            var wopiUrl = this.RemoveOptionalURLParamaters(urlTemplate);
            var separator = wopiUrl.Contains("?") ? '&' : '?';
            var viewingURL = wopiUrl + separator + queryString;

            this.trace.TraceInformation($"WopiURLHelper: Generated URL: ${viewingURL}");
            return viewingURL;
        }

        /// <summary>
        /// Strips out the optional parameters in viewing URL templates. 
        /// </summary>
        /// <param name="url">The full URI from the WOPI Client</param>
        /// <returns>A URI stripped of optional parameters</returns>
        private string RemoveOptionalURLParamaters(string url)
        {
            Contract.AssertNonEmpty(url, nameof(url), "url must be defined");

            // Viewing templates typically look like this 
            // <see cref="http://wopiclientserver.cloudapp.net/wv/WordPreviewHandler.ashx?
            // <ui=UI_LLCC&><rs=DC_LLCC&><showpagestats=PERFSTATS&>"/>
            // and this method will return <see cref="http://wopiclientserver.cloudapp.net/wv/WordPreviewHandler.ashx"/>
            return Regex.Replace(url, @"[&?]*(<.*>)$", string.Empty, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Gets the query string
        /// </summary>
        /// <param name="queryParams">Dictionary of query values</param>
        /// <returns>concatenated query string</returns>
        private string CreateQueryString(Dictionary<string, string> queryParams)
        {
            Contract.AssertValue(queryParams, nameof(queryParams));

            var queryEntries = new List<string>();
            var queryKeys = queryParams.Keys;
            foreach (var key in queryKeys)
            {
                var value = string.Empty;
                var exists = queryParams.TryGetValue(key, out value);
                if (exists)
                {
                    var queryString = $"{key}={value}";
                    queryEntries.Add(queryString);
                }
            }

            return $"{string.Join("&", queryEntries)}";
        }
    }
}
