//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.XrmHttp
{
    using System;
    using System.Net;
    using ServicePlatform.Exceptions;

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "Common.XrmHttp", nameof(XrmHttpClientRequestTimeoutException), MonitoredExceptionKind.Remote)]
    public class XrmHttpClientRequestTimeoutException : MonitoredException
    {
        public XrmHttpClientRequestTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
