//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Net;
    using MS.GTA.ServicePlatform.Exceptions;

    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, "MS.GTA.Common.XrmHttp", "XrmBackendDoesNotExistAtUri", MonitoredExceptionKind.Remote)]
    public class XrmEnvironmentBackendDoesNotExistAtUri : MonitoredException
    {
        public XrmEnvironmentBackendDoesNotExistAtUri(string backendUri, Exception innerException)
            : base($"Cannot connect to {backendUri}, the environment may be deleted or moved to a different uri", innerException)
        {
        }
    }
}
