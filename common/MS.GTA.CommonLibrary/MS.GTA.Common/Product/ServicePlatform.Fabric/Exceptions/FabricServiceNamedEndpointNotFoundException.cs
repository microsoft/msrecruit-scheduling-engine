//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Net;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Exceptions;

namespace MS.GTA.ServicePlatform.Fabric
{
    /// <summary>
    /// Thrown when a named endpoint cannot be found in a resolved endpoint address collection.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public sealed class FabricServiceNamedEndpointNotFoundException : MonitoredFabricException
    {
        internal FabricServiceNamedEndpointNotFoundException(Uri serviceName, string listenerName, string resolvedEndpointAddress)
            : base($"Named endpoint '{listenerName}' for service '{serviceName}' could not be found")
        {
            Contract.AssertValue(serviceName, nameof(serviceName));
            Contract.AssertValue(resolvedEndpointAddress, nameof(resolvedEndpointAddress));

            ServiceName = serviceName;
            ListenerName = listenerName;
            Address = resolvedEndpointAddress;
        }

        /// <summary>
        /// Gets the service name that was being resolved.
        /// </summary>
        [ExceptionCustomData]
        public Uri ServiceName { get; }

        /// <summary>
        /// Gets the named endpoint name.
        /// </summary>
        [ExceptionCustomData]
        public string ListenerName { get; }

        /// <summary>
        /// Gets resolved endpoint address.
        /// </summary>
        [ExceptionCustomData]
        internal string Address { get; }
    }
}
