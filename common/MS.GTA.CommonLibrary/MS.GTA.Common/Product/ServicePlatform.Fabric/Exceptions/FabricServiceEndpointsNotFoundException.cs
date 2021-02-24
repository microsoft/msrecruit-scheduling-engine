//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Fabric
{
    /// <summary>
    /// Thrown when service partition resolution does not return any endpoints.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public sealed class FabricServiceEndpointsNotFoundException : MonitoredFabricException
    {
        internal FabricServiceEndpointsNotFoundException(Uri serviceName)
            : base($"Service resolution for '{serviceName}' did not return any endpoints")
        {
            Contract.AssertValue(serviceName, nameof(serviceName));

            ServiceName = serviceName;
        }

        /// <summary>
        /// Gets the service name that was being resolved.
        /// </summary>
        [ExceptionCustomData]
        public Uri ServiceName { get; }
    }
}
