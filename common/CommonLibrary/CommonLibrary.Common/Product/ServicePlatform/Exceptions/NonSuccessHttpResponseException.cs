//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.ServicePlatform.Exceptions;
using CommonLibrary.ServicePlatform.Tracing;
using Microsoft.Extensions.Logging;

namespace CommonLibrary.ServicePlatform.Communication.Http
{
    /// <summary>
    /// An exception wrapping non-successful http responses.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote, true)]
    public sealed class NonSuccessHttpResponseException : HttpCommunicationException
    {
        public NonSuccessHttpResponseException(
            HttpStatusCode remoteStatusCode,
            string remoteReasonPhrase,
            ServiceError remoteServiceError,
            string content,
            HttpResponseHeaders headers,
            Exception innerException = null)
            : base("Server returned non-success HTTP response", remoteServiceError, innerException)
        {
            Content = content;
            Headers = headers;
            RemoteStatusCode = remoteStatusCode;
            RemoteReasonPhrase = remoteReasonPhrase;
        }

        /// <summary>
        /// The response status code.
        /// </summary>
        [ExceptionCustomData]
        public HttpStatusCode RemoteStatusCode { get; }

        /// <summary>
        /// The response reason phrase.
        /// </summary>
        [ExceptionCustomData]
        public string RemoteReasonPhrase { get; }

        /// <summary>Gets the content.</summary>
        public string Content { get; }

        /// <summary>Gets the headers.</summary>
        public HttpResponseHeaders Headers { get; }

        /// <summary>
        /// The remote service error namespace.
        /// </summary>
        [ExceptionCustomData]
        public string RemoteServiceErrorNamespace => RemoteServiceError?.ErrorNamespace;

        /// <summary>
        /// The remote service error code.
        /// </summary>
        [ExceptionCustomData]
        public string RemoteServiceErrorCode => RemoteServiceError?.ErrorCode;

        /// <summary>
        /// Creates a <see cref="NonSuccessHttpResponseException"/> exception for an unsuccessful HTTP response.
        /// </summary>
        public static async Task<NonSuccessHttpResponseException> CreateAsync(HttpResponseMessage httpResponseMessage, ILoggerFactory loggerFactory = null, string errorNameSpace = null)
        {
            Contract.CheckValue(httpResponseMessage, nameof(httpResponseMessage));
            Contract.Check(!httpResponseMessage.IsSuccessStatusCode, nameof(httpResponseMessage) + "." + nameof(httpResponseMessage.IsSuccessStatusCode));

            var logger = loggerFactory?.CreateLogger<NonSuccessHttpResponseException>();

            if (httpResponseMessage.Content == null)
            {
                Log("No response body even though the error payload header is present", logger);
            }

            string content = await httpResponseMessage.Content?.ReadAsStringAsync();

            ServiceError serviceError = null;
            IEnumerable<string> errorPayloadValues;
            if (httpResponseMessage.Headers.TryGetValues(HttpConstants.Headers.ErrorPayloadHeaderName, out errorPayloadValues))
            {
                serviceError = CreateServiceError(content, logger);
            }
            
            if (serviceError == null)
            {
                serviceError = new ServiceError(
                   errorNamespace: errorNameSpace ?? ErrorNamespaces.ServicePlatform,
                   errorCode: httpResponseMessage.StatusCode.ToString() ?? ErrorCodes.GenericServiceError,
                   message: httpResponseMessage.ReasonPhrase,
                   customData: Enumerable.Empty<CustomData>());
            }

            // Throw non success http response to the caller with the service error message.
            return new NonSuccessHttpResponseException(
                httpResponseMessage.StatusCode,
                httpResponseMessage.ReasonPhrase,
                serviceError,
                content,
                httpResponseMessage.Headers);
        }

        public static ServiceError CreateServiceError(string httpResponseMessage, ILogger logger = null)
        {
            var errorDeserializer = new ServiceErrorJsonSerializer();
            if (!errorDeserializer.TryDeserialize(httpResponseMessage, out var serviceError))
                Log("Failed to deserialize Remote Service Error", logger);

            return serviceError;
        }

        private static void Log(string msg, ILogger logger = null)
        {
            if (logger == null)
            {
                ServicePlatformTrace.Instance.TraceWarning(msg);
            }
            else
            {
                logger.LogWarning(msg);
            }
        }
    }
}
