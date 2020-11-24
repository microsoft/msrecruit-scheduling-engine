//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MS.GTA.CommonDataService.Common.Internal;

namespace MS.GTA.ServicePlatform.Communication.Http
{
    /// <summary>
    /// Convenience extension methods over <see cref="IHttpCommunicationClient"/>.
    /// </summary>
    public static class HttpCommunicationClientExtensions
    {
        private static readonly TimeSpan DefaultRetryAfter = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Issues an HTTP GET request against the provided <paramref name="requestUri"/>.
        /// </summary>
        public static Task<HttpResponseMessage> GetAsync(this IHttpCommunicationClient httpServiceClient, string requestUri, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.GetAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), null, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP GET request against the provided <paramref name="requestUri"/>.
        /// </summary>
        public static Task<HttpResponseMessage> GetAsync(this IHttpCommunicationClient httpServiceClient, string requestUri, IEnumerable<KeyValuePair<string, IEnumerable<string>>> customHeaders, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.GetAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), customHeaders, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP GET request against the provided <paramref name="requestUri"/>.
        /// </summary>
        public static Task<HttpResponseMessage> GetAsync(this IHttpCommunicationClient httpServiceClient, Uri requestUri, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.GetAsync(requestUri, null, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP GET request against the provided <paramref name="requestUri"/>.
        /// </summary>
        public static async Task<HttpResponseMessage> GetAsync(this IHttpCommunicationClient httpServiceClient, Uri requestUri, IEnumerable<KeyValuePair<string, IEnumerable<string>>> customHeaders, CancellationToken cancellationToken)
        {
            Contract.CheckValue(httpServiceClient, nameof(httpServiceClient));
            Contract.CheckValue(requestUri, nameof(requestUri));

            using (var request = new HttpRequestMessage(HttpMethod.Get, requestUri))
            {
                request.AddCustomHeaders(customHeaders);
                return await httpServiceClient.SendAsync(request, cancellationToken);
            }
        }

        /// <summary>
        /// Issues an HTTP DELETE request against the provided <paramref name="requestUri"/>.
        /// </summary>
        public static Task<HttpResponseMessage> DeleteAsync(this IHttpCommunicationClient httpServiceClient, string requestUri, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.DeleteAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), null, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP DELETE request against the provided <paramref name="requestUri"/>.
        /// </summary>
        public static Task<HttpResponseMessage> DeleteAsync(this IHttpCommunicationClient httpServiceClient, string requestUri, IEnumerable<KeyValuePair<string, IEnumerable<string>>> customHeaders, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.DeleteAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), customHeaders, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP DELETE request against the provided <paramref name="requestUri"/>.
        /// </summary>
        public static Task<HttpResponseMessage> DeleteAsync(this IHttpCommunicationClient httpServiceClient, Uri requestUri, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.DeleteAsync(requestUri, null, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP DELETE request against the provided <paramref name="requestUri"/>.
        /// </summary>
        public static async Task<HttpResponseMessage> DeleteAsync(this IHttpCommunicationClient httpServiceClient, Uri requestUri, IEnumerable<KeyValuePair<string, IEnumerable<string>>> customHeaders, CancellationToken cancellationToken)
        {
            Contract.CheckValue(httpServiceClient, nameof(httpServiceClient));
            Contract.CheckValue(requestUri, nameof(requestUri));

            using (var request = new HttpRequestMessage(HttpMethod.Delete, requestUri))
            {
                request.AddCustomHeaders(customHeaders);
                return await httpServiceClient.SendAsync(request, cancellationToken);
            }
        }

        /// <summary>
        /// Issues an HTTP POST request against the provided <paramref name="requestUri"/> with the provided <paramref name="content"/>.
        /// 
        /// The extension method does not dispose the provided <paramref name="content"/>.
        /// </summary>
        public static Task<HttpResponseMessage> PostAsync(this IHttpCommunicationClient httpServiceClient, string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.PostAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), content, null, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP POST request against the provided <paramref name="requestUri"/> with the provided <paramref name="content"/>.
        /// 
        /// The extension method does not dispose the provided <paramref name="content"/>.
        /// </summary>
        public static Task<HttpResponseMessage> PostAsync(this IHttpCommunicationClient httpServiceClient, string requestUri, HttpContent content, IEnumerable<KeyValuePair<string, IEnumerable<string>>> customHeaders, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.PostAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), content, customHeaders, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP POST request against the provided <paramref name="requestUri"/> with the provided <paramref name="content"/>.
        /// 
        /// The extension method does not dispose the provided <paramref name="content"/>.
        /// </summary>
        public static Task<HttpResponseMessage> PostAsync(this IHttpCommunicationClient httpServiceClient, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.PostAsync(requestUri, content, null, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP POST request against the provided <paramref name="requestUri"/> with the provided <paramref name="content"/>.
        /// 
        /// The extension method does not dispose the provided <paramref name="content"/>.
        /// </summary>
        public static Task<HttpResponseMessage> PostAsync(this IHttpCommunicationClient httpServiceClient, Uri requestUri, HttpContent content, IEnumerable<KeyValuePair<string, IEnumerable<string>>> customHeaders, CancellationToken cancellationToken)
        {
            Contract.CheckValue(httpServiceClient, nameof(httpServiceClient));
            Contract.CheckValue(requestUri, nameof(requestUri));
            Contract.CheckValue(content, nameof(content));

            // Do not dispose the request message as we don't own the content
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);
            request.AddCustomHeaders(customHeaders);
            request.Content = content;

            return httpServiceClient.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP PUT request against the provided <paramref name="requestUri"/> with the provided <paramref name="content"/>.
        ///
        /// The extension method does not dispose the provided <paramref name="content"/>.
        /// </summary>
        public static Task<HttpResponseMessage> PutAsync(this IHttpCommunicationClient httpServiceClient, string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.PutAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), content, null, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP PUT request against the provided <paramref name="requestUri"/> with the provided <paramref name="content"/>.
        ///
        /// The extension method does not dispose the provided <paramref name="content"/>.
        /// </summary>
        public static Task<HttpResponseMessage> PutAsync(this IHttpCommunicationClient httpServiceClient, string requestUri, HttpContent content, IEnumerable<KeyValuePair<string, IEnumerable<string>>> customHeaders, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.PutAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), content, customHeaders, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP PUT request against the provided <paramref name="requestUri"/> with the provided <paramref name="content"/>.
        ///
        /// The extension method does not dispose the provided <paramref name="content"/>.
        /// </summary>
        public static Task<HttpResponseMessage> PutAsync(this IHttpCommunicationClient httpServiceClient, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.PutAsync(requestUri, content, null, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP PUT request against the provided <paramref name="requestUri"/> with the provided <paramref name="content"/>.
        ///
        /// The extension method does not dispose the provided <paramref name="content"/>.
        /// </summary>
        public static Task<HttpResponseMessage> PutAsync(this IHttpCommunicationClient httpServiceClient, Uri requestUri, HttpContent content, IEnumerable<KeyValuePair<string, IEnumerable<string>>> customHeaders, CancellationToken cancellationToken)
        {
            Contract.CheckValue(httpServiceClient, nameof(httpServiceClient));
            Contract.CheckValue(requestUri, nameof(requestUri));
            Contract.CheckValue(content, nameof(content));

            // Do not dispose the request message as we don't own the content
            var request = new HttpRequestMessage(HttpMethod.Put, requestUri);
            request.AddCustomHeaders(customHeaders);
            request.Content = content;

            return httpServiceClient.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP PATCH request against the provided <paramref name="requestUri"/> with the provided <paramref name="content"/>.
        ///
        /// The extension method does not dispose the provided <paramref name="content"/>.
        /// </summary>
        public static Task<HttpResponseMessage> PatchAsync(this IHttpCommunicationClient httpServiceClient, string requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.PatchAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), content, null, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP PATCH request against the provided <paramref name="requestUri"/> with the provided <paramref name="content"/>.
        ///
        /// The extension method does not dispose the provided <paramref name="content"/>.
        /// </summary>
        public static Task<HttpResponseMessage> PatchAsync(this IHttpCommunicationClient httpServiceClient, string requestUri, HttpContent content, IEnumerable<KeyValuePair<string, IEnumerable<string>>> customHeaders, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.PatchAsync(new Uri(requestUri, UriKind.RelativeOrAbsolute), content, customHeaders, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP PATCH request against the provided <paramref name="requestUri"/> with the provided <paramref name="content"/>.
        ///
        /// The extension method does not dispose the provided <paramref name="content"/>.
        /// </summary>
        public static Task<HttpResponseMessage> PatchAsync(this IHttpCommunicationClient httpServiceClient, Uri requestUri, HttpContent content, CancellationToken cancellationToken)
        {
            Contract.CheckValue(requestUri, nameof(requestUri));
            return httpServiceClient.PatchAsync(requestUri, content, null, cancellationToken);
        }

        /// <summary>
        /// Issues an HTTP PATCH request against the provided <paramref name="requestUri"/> with the provided <paramref name="content"/>.
        ///
        /// The extension method does not dispose the provided <paramref name="content"/>.
        /// </summary>
        public static Task<HttpResponseMessage> PatchAsync(this IHttpCommunicationClient httpServiceClient, Uri requestUri, HttpContent content, IEnumerable<KeyValuePair<string, IEnumerable<string>>> customHeaders, CancellationToken cancellationToken)
        {
            Contract.CheckValue(httpServiceClient, nameof(httpServiceClient));
            Contract.CheckValue(requestUri, nameof(requestUri));
            Contract.CheckValue(content, nameof(content));

            // Do not dispose the request message as we don't own the content
            var request = new HttpRequestMessage(new HttpMethod("PATCH"), requestUri);
            request.AddCustomHeaders(customHeaders);
            request.Content = content;

            return httpServiceClient.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// If HTTP Response <paramref name="responseMessage"/> is Accepted and contains a location header 
        /// it will send subsequent HTTP requests to get status using the location URI value in the header with polling interval defined by <paramref name="defaultRetryAfter"/> and
        /// timeout <paramref name="timeout"/>.
        /// </summary>
        ///<remarks>The HTTP response message returned by the server for operations in-progress or not started must have HTTP status code 202 (Accepted), or else accessing the response object will result in a System.ObjectDisposedException.</remarks>  
        ///<remarks>The response object returned by this method should be disposed by the caller.</remarks>  
        public static async Task<HttpResponseMessage> PollLocationHeaderUntilNonAcceptedStatusAsync(this IHttpCommunicationClient httpServiceClient, HttpResponseMessage responseMessage, TimeSpan timeout, CancellationToken cancellationToken,
            TimeSpan? defaultRetryAfter = null)
        {
            Contract.CheckValue(httpServiceClient, nameof(httpServiceClient));
            Contract.CheckValue(responseMessage, nameof(responseMessage));

            if (responseMessage.StatusCode == HttpStatusCode.Accepted && responseMessage.Headers.Location != null)
            {
                TimeSpan? retryAfter = defaultRetryAfter;
                if (responseMessage.Headers.RetryAfter != null)
                {
                    retryAfter = responseMessage.Headers.RetryAfter.Delta;
                }

                return await PollUriUntilNonAcceptedStatusAsync(httpServiceClient, responseMessage.Headers.Location, retryAfter, timeout, cancellationToken);
            }

            // HTTP Response is not Accepted or does not contain a location header, return response as is
            return responseMessage;
        }

        /// <summary>
        /// It polls for status using <paramref name="getStatusUri"/> with polling interval defined by <paramref name="retryAfter"/> and timeout <paramref name="timeout"/>.
        /// The response returned by this method should be disposed by the caller.
        /// </summary>
        ///<remarks>The HTTP response message returned by the server for operations in-progress or not started must have HTTP status code 202 (Accepted), or else accessing the response object will result in a System.ObjectDisposedException.</remarks>  
        ///<remarks>The response object returned by this method should be disposed by the caller.</remarks>  
        public static async Task<HttpResponseMessage> PollUriUntilNonAcceptedStatusAsync(IHttpCommunicationClient httpServiceClient, Uri getStatusUri, TimeSpan? retryAfter, TimeSpan timeout, CancellationToken cancellationToken)
        {
            Contract.CheckValue(httpServiceClient, nameof(httpServiceClient));
            Contract.CheckValue(getStatusUri, nameof(getStatusUri));

            int retryCount = 0;
            DateTime maxTime = DateTime.UtcNow.Add(timeout);
            do
            {
                if (!retryAfter.HasValue)
                {
                    retryAfter = DefaultRetryAfter;
                }

                // Use default polling interval if the server gave no retryAfter response header.
                await Task.Delay(retryAfter.Value, cancellationToken);

                // Make request and check response
                HttpResponseMessage response = null;
                try
                {
                    response = await httpServiceClient.GetAsync(getStatusUri, cancellationToken);
                    if (response.StatusCode != HttpStatusCode.Accepted)
                    {
                        return response;
                    }

                    if (response.Headers?.Location != null)
                    {
                        getStatusUri = response.Headers.Location;
                    }
                    if (response.Headers?.RetryAfter != null)
                    {
                        retryAfter = response.Headers.RetryAfter.Delta;
                    }
                }
                finally
                {
                    // Always dispose the response object unless it contains a 'final' state for the long running operation
                    // which case the caller will have to dispose it.
                    // This assumes the HTTP response status for not started or in-progress operations is 202 (Accepted).
                    if (response?.StatusCode == HttpStatusCode.Accepted)
                    {
                        response.Dispose();
                    }
                }
                
                retryCount += 1;
            }
            while (maxTime >= DateTime.UtcNow);

            // Max allowed time reached, raise timeout exception
            throw new TimeoutException($"Waiting for the long running operation at {getStatusUri} timed out after {timeout.TotalSeconds} seconds. Retried {retryCount} times.");
        }

        private static void AddCustomHeaders(this HttpRequestMessage request, IEnumerable<KeyValuePair<string, IEnumerable<string>>> customHeaders)
        {
            if (customHeaders == null)
            {
                return;
            }

            foreach (var header in customHeaders)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }
    }
}
