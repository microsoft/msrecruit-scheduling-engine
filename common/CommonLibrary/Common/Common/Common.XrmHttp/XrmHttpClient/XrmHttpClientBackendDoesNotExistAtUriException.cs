//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.XrmHttp
{
    using System;
    using System.Net;
    using ServicePlatform.Exceptions;

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "Common.XrmHttp", "XrmBackendDoesNotExistAtUri", MonitoredExceptionKind.Remote)]
    public class XrmEnvironmentBackendDoesNotExistAtUri : MonitoredException
    {
        public XrmEnvironmentBackendDoesNotExistAtUri(string backendUri, Exception innerException)
            : base($"Cannot connect to {backendUri}, the environment may be deleted or moved to a different uri", innerException)
        {
        }
    }
}
