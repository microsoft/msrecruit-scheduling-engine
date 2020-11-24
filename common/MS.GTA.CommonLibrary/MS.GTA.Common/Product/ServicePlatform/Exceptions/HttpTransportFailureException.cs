// ----------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation. All rights reserved.
// ----------------------------------------------------------------------------

using System;
using System.Net;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Communication.Http
{
    /// <summary>
    /// A convenience exception for all request/response transport failures. When this exception is thrown 
    /// it means that a transport error occurred while the request or response data was being sent. The 
    /// target server may or may not have received a portion or the full request data.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public sealed class HttpTransportFailureException : HttpCommunicationWebException
    {
        internal HttpTransportFailureException(WebExceptionStatus webExceptionStatus, Exception innerException = null)
            : base("A transport error occurred while sending the HTTP request or receiving the response", webExceptionStatus, innerException)
        {
        }
    }
}
