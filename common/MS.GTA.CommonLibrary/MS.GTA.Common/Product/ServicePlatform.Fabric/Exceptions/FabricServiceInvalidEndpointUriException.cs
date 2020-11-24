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
    /// Thrown when a resolved endpoint address is not a valid absolute URL.
    /// </summary>
    [Serializable]
    [MonitoredExceptionMetadata(HttpStatusCode.InternalServerError, ErrorNamespaces.ServicePlatform, ErrorCodes.GenericServiceError, MonitoredExceptionKind.Remote)]
    public sealed class FabricServiceInvalidEndpointUriException : MonitoredFabricException
    {
        internal FabricServiceInvalidEndpointUriException(Uri serviceName, string listenerName, string endpointUri)
            : base($"Named endpoint '{listenerName}' URI '{endpointUri}' for service '{serviceName}' is not a valid HTTP service endpoint")
        {
            Contract.AssertValue(serviceName, nameof(serviceName));
            Contract.AssertValue(endpointUri, nameof(endpointUri));

            ServiceName = serviceName;
            ListenerName = listenerName;
            EndpointUri = endpointUri;
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
        /// Gets endpoint URI.
        /// </summary>
        [ExceptionCustomData]
        internal string EndpointUri { get; }
    }
}
