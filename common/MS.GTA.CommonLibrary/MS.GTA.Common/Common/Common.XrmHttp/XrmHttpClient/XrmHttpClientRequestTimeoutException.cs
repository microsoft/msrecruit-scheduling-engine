// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Net;
    using MS.GTA.ServicePlatform.Exceptions;

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", nameof(XrmHttpClientRequestTimeoutException), MonitoredExceptionKind.Remote)]
    public class XrmHttpClientRequestTimeoutException : MonitoredException
    {
        public XrmHttpClientRequestTimeoutException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
