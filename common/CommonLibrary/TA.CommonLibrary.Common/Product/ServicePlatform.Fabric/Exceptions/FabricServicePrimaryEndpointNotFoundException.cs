//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Fabric;
using System.Linq;
using System.Net;
using TA.CommonLibrary.CommonDataService.Common.Internal;
using TA.CommonLibrary.ServicePlatform.Exceptions;

namespace TA.CommonLibrary.ServicePlatform.Fabric
{
    /// <summary>
    /// Thrown when a required primary service endpoint doesn't exist for a stateful service.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public sealed class FabricServicePrimaryEndpointNotFoundException : MonitoredFabricException
    {
        internal FabricServicePrimaryEndpointNotFoundException(Uri serviceName, ICollection<ResolvedServiceEndpoint> resolvedEndpoints)
            : base($"Primary endpoint for service '{serviceName}' does not exist")
        {
            Contract.AssertValue(serviceName, nameof(serviceName));
            Contract.AssertValue(resolvedEndpoints, nameof(resolvedEndpoints));

            ServiceName = serviceName;
            ResolvedEndpoints = new ReadOnlyCollection<ResolvedServiceEndpoint>(resolvedEndpoints.ToList());

            var resolvedEndpointStrings = resolvedEndpoints.Select(e => $"{e.Role}Endpoint: {e.Address}");
            ResolvedEndpointsString = $"{string.Join("; ", resolvedEndpointStrings)}";
        }

        /// <summary>
        /// Gets the service name that was being resolved.
        /// </summary>
        [ExceptionCustomData]
        public Uri ServiceName { get; }

        /// <summary>
        /// Gets the collection of resolved service endpoints.
        /// </summary>
        public IReadOnlyList<ResolvedServiceEndpoint> ResolvedEndpoints { get; }

        /// <summary>
        /// Gets the string for trace serialization.
        /// </summary>
        [ExceptionCustomData(Name = "ResolvedEndpoints")]
        internal string ResolvedEndpointsString { get; }
    }
}
