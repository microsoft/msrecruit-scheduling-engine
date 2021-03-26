//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.XrmHttp
{
    using System;
    using System.Net;
    using TA.CommonLibrary.ServicePlatform.Exceptions;

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "TA.CommonLibrary.Common.XrmHttp", nameof(XrmHttpClientRequestTimeoutException), MonitoredExceptionKind.Remote)]
    public class XrmHttpClientRequestTimeoutException : MonitoredException
    {
        public XrmHttpClientRequestTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
