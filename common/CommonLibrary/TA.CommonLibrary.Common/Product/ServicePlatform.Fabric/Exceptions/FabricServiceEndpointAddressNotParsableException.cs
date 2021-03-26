//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Net;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Exceptions;

namespace TA.CommonLibrary.ServicePlatform.Fabric
{
    /// <summary>
    /// Thrown when a resolved endpoint is not parsable into an address collection.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public sealed class FabricServiceEndpointAdressNotParsableException : MonitoredFabricException
    {
        internal FabricServiceEndpointAdressNotParsableException(Uri serviceName, string resolvedEndpointAddress)
            : base($"Endpoint address for service '{serviceName}' is not parsable")
        {
            Contract.AssertValue(serviceName, nameof(serviceName));
            Contract.AssertValue(resolvedEndpointAddress, nameof(resolvedEndpointAddress));

            ServiceName = serviceName;
            Address = resolvedEndpointAddress;
        }

        /// <summary>
        /// Gets the service name that was being resolved.
        /// </summary>
        [ExceptionCustomData]
        public Uri ServiceName { get; }

        /// <summary>
        /// Gets resolved endpoint address.
        /// </summary>
        [ExceptionCustomData]
        internal string Address { get; }
    }
}
