//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp
{
    using System;
    using System.Net;
    using HR.TA.ServicePlatform.Exceptions;

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "HR.TA.Common.XrmHttp", nameof(XrmHttpClientRequestTimeoutException), MonitoredExceptionKind.Remote)]
    public class XrmHttpClientRequestTimeoutException : MonitoredException
    {
        public XrmHttpClientRequestTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
