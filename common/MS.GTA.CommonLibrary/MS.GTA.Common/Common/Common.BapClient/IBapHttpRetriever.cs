//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.BapClient
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// Creates an HTTP helper to make BAP calls.
    /// </summary>
    public interface IBapHttpRetriever
    {
        /// <summary>The attach API version.</summary>
        /// <param name="url">The url.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string AttachApiVersion(string url);

        /// <summary>Attaches the XRM API version.</summary>
        /// <param name="url">The url.</param>
        /// <returns>The <see cref="string"/>.</returns>
        string AttachXRMApiVersion(string url);

        /// <summary>The append query params.</summary>
        /// <param name="url">The url.</param>
        /// <param name="apiVersion">The API version to use</param>
        /// <param name="expand">The expand option to use</param>
        /// <returns>The <see cref="string"/>.</returns>
        string AppendQueryParams(string url, string apiVersion = null, string expand = null);

        /// <summary>The send async.</summary>
        /// <param name="url">The url.</param>
        /// <param name="tenantId">The tenant Id.</param>
        /// <param name="userObjectId">The user object id</param>
        /// <param name="content">The post message content.</param>
        /// <param name="method">The method.</param>
        /// <param name="retryOnTransientErrors">Retry on transient errors.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<HttpResponseMessage> SendAsServiceAsync(string url, string tenantId, string userObjectId = null, HttpContent content = null, HttpMethod method = null, bool retryOnTransientErrors = false);

        /// <summary>The send as user async.</summary>
        /// <param name="url">The url.</param>
        /// <param name="tenantId">The tenant Id.</param>
        /// <param name="userObjectId">The user Object Id.</param>
        /// <param name="content">The content.</param>
        /// <param name="method">The method.</param>
        /// <param name="retryOnTransientErrors">Retry on transient errors.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<HttpResponseMessage> SendAsUserAsync(string url, string tenantId, string userObjectId, HttpContent content = null, HttpMethod method = null, bool retryOnTransientErrors = false);

        /// <summary>The send user async.</summary>
        /// <param name="url">The url.</param>
        /// <param name="token">The token.</param>
        /// <param name="method">The method.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task<HttpResponseMessage> SendWithTokenAsync(string url, string token, HttpMethod method = null);
    }
}
