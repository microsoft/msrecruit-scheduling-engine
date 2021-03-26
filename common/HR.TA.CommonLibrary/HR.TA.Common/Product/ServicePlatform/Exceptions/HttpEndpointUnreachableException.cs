//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net;
using HR.TA.ServicePlatform.Exceptions;

namespace HR.TA.ServicePlatform.Communication.Http
{
    /// <summary>
    /// A convenience exception for all connection failures. When this exception is thrown 
    /// it means that no portion of the request message was sent as the error happened before
    /// the TCP connection was established.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public sealed class HttpEndpointUnreachableException : HttpCommunicationWebException
    {
        internal HttpEndpointUnreachableException(WebExceptionStatus webExceptionStatus, Exception innerException = null)
            : base("A connection error occurred while sending the HTTP request", webExceptionStatus, innerException)
        {
        }
    }
}
