//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using CommonDataService.Common.Internal;
using Microsoft.ServiceFabric.Services.Client;

namespace ServicePlatform.Communication.Http.Routers
{
    /// <summary>
    /// Identifies a Fabric service endpoint
    /// </summary>
    public sealed class FabricServiceEndpoint
    {
        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceEndpoint"/> for unnamed listener in singleton partition service.
        /// </summary>
        public FabricServiceEndpoint(Uri serviceUri)
            : this(serviceUri, listenerName: string.Empty)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceEndpoint"/> for named listener in singleton partition service.
        /// </summary>
        public FabricServiceEndpoint(Uri serviceUri, string listenerName)
            : this(serviceUri, partitionKey: ServicePartitionKey.Singleton, listenerName: listenerName)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceEndpoint"/> for unnamed listener in specific service partition.
        /// </summary>
        public FabricServiceEndpoint(Uri serviceUri, ServicePartitionKey partitionKey)
            : this(serviceUri, partitionKey, listenerName: string.Empty)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceEndpoint"/> for named listener in specific service partition.
        /// </summary>
        public FabricServiceEndpoint(Uri serviceUri, ServicePartitionKey partitionKey, string listenerName)
        {
            Contract.CheckValue(serviceUri, nameof(serviceUri));
            Contract.Check(serviceUri.IsAbsoluteUri, nameof(serviceUri) + " should be an absolute URI");
            Contract.CheckValue(partitionKey, nameof(partitionKey));
            Contract.CheckValue(listenerName, nameof(listenerName)); // Always requiring this to avoid magic endpoint selection based on order

            ServiceUri = serviceUri;
            PartitionKey = partitionKey;
            ListenerName = listenerName;
        }

        /// <summary>
        /// Gets the Fabric service URI (e.g. "fabric:/Application/Service").
        /// </summary>
        public Uri ServiceUri { get; }

        /// <summary>
        /// Gets the Fabric partition key.
        /// </summary>
        public ServicePartitionKey PartitionKey { get; }

        /// <summary>
        /// Gets the target listener name as reported to the Naming service.
        /// </summary>
        public string ListenerName { get; }
    }
}
