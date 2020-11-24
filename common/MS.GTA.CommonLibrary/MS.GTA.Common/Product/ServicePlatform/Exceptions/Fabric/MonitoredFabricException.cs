//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System;
using System.Net;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Fabric
{
    /// <summary>
    /// Base class for monitored Fabric exceptions.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public class MonitoredFabricException : MonitoredException
    {
        internal MonitoredFabricException(Exception innerException)
            : base("A FabricException occurred", innerException: innerException)
        {
        }

        internal MonitoredFabricException(string message)
            : base(message, innerException: null)
        {
        }

        internal MonitoredFabricException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
