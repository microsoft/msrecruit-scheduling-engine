//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net;
using CommonDataService.Common.Internal;
using ServicePlatform.Exceptions;

namespace ServicePlatform.Fabric
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
