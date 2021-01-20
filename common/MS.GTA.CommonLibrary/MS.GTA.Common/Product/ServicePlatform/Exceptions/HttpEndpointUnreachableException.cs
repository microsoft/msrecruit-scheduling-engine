//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Net;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Communication.Http
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
