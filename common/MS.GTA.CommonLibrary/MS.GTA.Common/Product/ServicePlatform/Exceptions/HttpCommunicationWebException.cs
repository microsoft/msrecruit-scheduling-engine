//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System;
using System.Net;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Communication.Http
{
    /// <summary>
    /// A convenience exception for wrapping <see cref="WebException"/>s thrown 
    /// from the underlying communication stack.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public class HttpCommunicationWebException : HttpCommunicationException
    {
        internal HttpCommunicationWebException(WebExceptionStatus webExceptionStatus, Exception innerException = null)
            : this("An error occurred while sending the HTTP request", webExceptionStatus, innerException)
        {
        }

        internal HttpCommunicationWebException(string message, WebExceptionStatus webExceptionStatus, Exception innerException = null)
            : base(message, innerException)
        {
            WebExceptionStatus = webExceptionStatus;
        }

        /// <summary>
        /// The <see cref="WebExceptionStatus"/> from the original <see cref="WebException"/>.
        /// </summary>
        [ExceptionCustomData]
        public WebExceptionStatus WebExceptionStatus { get; }
    }
}
