// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ExternalApiException.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Web.Exceptions
{
    using System.Net;
    using System.Net.Http.Headers;    
    using MS.GTA.ServicePlatform.Exceptions;

    /// <summary>The external API exception.</summary>
    /// TODO: Ideally we would make the kind "Remote" but jarvis does not monitor for that so we'll leave it as service for now.
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "ExternalDependencyFailure", "ExternalApiFailure", MonitoredExceptionKind.Service)]
    public class ExternalApiException : MonitoredException
    {
        /// <summary>Initializes a new instance of the <see cref="ExternalApiException"/> class.</summary>
        /// <param name="content">The content.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="statusCode">The status Code.</param>
        public ExternalApiException(string content, HttpResponseHeaders headers, HttpStatusCode statusCode)
        {
            this.HttpStatusCode = statusCode;
            this.Headers = headers;
            this.Content = content;
        }

        /// <summary>Gets the content.</summary>
        public string Content { get; }

        /// <summary>Gets the headers.</summary>
        public HttpResponseHeaders Headers { get; }

        /// <summary>Gets the http status code.</summary>
        public HttpStatusCode HttpStatusCode { get; }
    }

    /// <summary>
    /// ExternalApi Task cancelled exceeption
    /// </summary>
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "ExternalDependencyFailure", "ExternalApiTaskCancelledFailure", MonitoredExceptionKind.Benign)]
    public sealed class ExternalApiTaskCancelledException : MonitoredException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalApiTaskCancelledException" /> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        public ExternalApiTaskCancelledException(string message)
            : base(message)
        {

        }
    }
}
