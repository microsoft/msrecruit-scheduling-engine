//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HR.TA.CommonDataService.Common.Internal;
using HR.TA.ServicePlatform.Communication.Http.Routers;
using HR.TA.ServicePlatform.Context;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication;
using PlatformServiceContext = HR.TA.ServicePlatform.Context.ServiceContext;

namespace HR.TA.ServicePlatform.Fabric.Communication.Internal
{
    internal sealed class FabricServiceRouterRequest : IRetriableRouterRequest
    {
        internal static ConcurrentDictionary<string, ResolvedServicePartition> resolvedServicePartitions = new ConcurrentDictionary<string, ResolvedServicePartition>();

        private static readonly Random random = new Random();

        private readonly IServicePartitionResolver servicePartitionResolver;
        private readonly FabricServiceEndpoint serviceEndpoint;
        private readonly FabricServiceRouterOptions fabricRouterOptions;
        private readonly ILogger logger;

        internal FabricServiceRouterRequest(
            IServicePartitionResolver servicePartitionResolver,
            FabricServiceEndpoint serviceEndpoint,
            FabricServiceRouterOptions fabricRouterOptions,
            ILoggerFactory loggerFactory = null)
        {
            Contract.AssertValue(servicePartitionResolver, nameof(servicePartitionResolver));
            Contract.AssertValue(serviceEndpoint, nameof(serviceEndpoint));
            Contract.AssertValue(fabricRouterOptions, nameof(fabricRouterOptions));

            this.servicePartitionResolver = servicePartitionResolver;
            this.serviceEndpoint = serviceEndpoint;
            this.fabricRouterOptions = fabricRouterOptions;
            this.logger = loggerFactory?.CreateLogger<FabricServiceRouterRequest>();
        }

        /// <inheritdoc />
        public bool ShouldRetry(HttpRequestMessage request, HttpResponseMessage lastResponse)
        {
            Contract.CheckValue(lastResponse, nameof(lastResponse));

            if (lastResponse.StatusCode == HttpStatusCode.Gone)
            {
                this.Log(
                    "Fabric service request should be retried because HTTP 410 response was received");
                return true;
            }
            else if (lastResponse.StatusCode == HttpStatusCode.NotFound)
            {
                if (!fabricRouterOptions.RetryOnNotFound)
                {
                    this.Log(
                        "Fabric service request should not be retried because HTTP 404 response was received and RetryOnNotFound is not set");
                    return false;
                }

                IEnumerable<string> serviceFabricHeaderValues;
                if (!lastResponse.Headers.TryGetValues(FabricConstants.Http.ServiceFabricHeaderName, out serviceFabricHeaderValues))
                {
                    this.Log(
                        "Fabric service request should be retried because the received HTTP 404 response does not contain X-ServiceFabric header");
                    return true;
                }

                if (!serviceFabricHeaderValues.Contains(FabricConstants.Http.ResourceNotFoundHeaderValue, StringComparer.OrdinalIgnoreCase))
                {
                    this.Log(
                        "Fabric service request should be retried because the received HTTP 404 response does not contain ResourceNotFound in the X-ServiceFabric header");
                    return true;
                }

                this.Log(
                    "Fabric service request should not be retried because the received HTTP 404 response contains ResourceNotFound in the X-ServiceFabric header");
                return false;
            }

            this.Log(
                "Fabric service request should not be retried because the received response status is neither HTTP 410 nor HTTP 404", LogLevel.Debug);
            return false;
        }

        /// <inheritdoc />
        public Task<Uri> GetNextEndpointAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (this.logger == null)
            {
                return PlatformServiceContext.Activity.ExecuteAsync(
                    FabricServiceRouterActivityType.Instance,
                    async () => await this.GetNextEndpointAsyncInternal(cancellationToken));
            }
            else
            {
                return this.logger.ExecuteAsync(
                    FabricServiceRouterActivityType.Instance,
                    async () => await this.GetNextEndpointAsyncInternal(cancellationToken),
                    new [] { typeof(MonitoredFabricException) });
            }

        }

        private async Task<Uri> GetNextEndpointAsyncInternal(CancellationToken cancellationToken)
        {
            try
            {
                var key =
                    $"{serviceEndpoint?.ServiceUri}-{serviceEndpoint?.PartitionKey?.Kind}-{serviceEndpoint?.PartitionKey?.Value}-{serviceEndpoint?.ListenerName}";

                if (resolvedServicePartitions.TryGetValue(key, out var lastResolvedServicePartition))
                {
                    this.Log($"Re-resolving service partition: ServiceUri={serviceEndpoint.ServiceUri}, ServicePartitionKey={serviceEndpoint.PartitionKey.Value}");
                    var reResolvedServicePartition = await servicePartitionResolver.ResolveAsync(
                        lastResolvedServicePartition,
                        fabricRouterOptions.ResolveTimeoutPerTry,
                        fabricRouterOptions.MaxResolveRetryBackoffInterval,
                        cancellationToken);

                    resolvedServicePartitions.TryUpdate(
                        key,
                        reResolvedServicePartition,
                        lastResolvedServicePartition);
                    lastResolvedServicePartition = reResolvedServicePartition;
                }
                else
                {
                    this.Log($"Resolving service partition: ServiceUri={serviceEndpoint.ServiceUri}, ServicePartitionKey={serviceEndpoint.PartitionKey.Value}");
                    lastResolvedServicePartition = await servicePartitionResolver.ResolveAsync(
                        serviceEndpoint.ServiceUri,
                        serviceEndpoint.PartitionKey,
                        fabricRouterOptions.ResolveTimeoutPerTry,
                        fabricRouterOptions.MaxResolveRetryBackoffInterval,
                        cancellationToken);
                    resolvedServicePartitions.TryAdd(key, lastResolvedServicePartition);
                }

                this.Log($"Resolved service partition");

                return GetEndpoint(lastResolvedServicePartition);
            }
            catch (FabricException ex)
            {
                this.Log(ex.ToString(), LogLevel.Error);
                throw new MonitoredFabricException(ex);
            }
        }

        private Uri GetEndpoint(ResolvedServicePartition resolvedServicePartition)
        {
            Contract.AssertValue(resolvedServicePartition, nameof(resolvedServicePartition));

            ResolvedServiceEndpoint targetEndpoint;

            var rspEndpoints = resolvedServicePartition.Endpoints;
            if (rspEndpoints == null || rspEndpoints.Count == 0)
            {
                throw new FabricServiceEndpointsNotFoundException(resolvedServicePartition.ServiceName);
            }

            // Pick any endpoint to determine the service type
            if (rspEndpoints.First().Role == ServiceEndpointRole.Stateless)
            {
                // If stateless then pick any random endpoint
                this.Log($"Picking a random resolved service endpoint for stateless service '{resolvedServicePartition.ServiceName}'", LogLevel.Debug);
                targetEndpoint = rspEndpoints.ElementAt(random.Next(rspEndpoints.Count));
            }
            else
            {
                this.Log($"Picking primary resolved service endpoint for stateful service '{resolvedServicePartition.ServiceName}'", LogLevel.Debug);
                targetEndpoint = rspEndpoints.FirstOrDefault(e => e.Role == ServiceEndpointRole.StatefulPrimary);
                if (targetEndpoint == null)
                {
                    // The primary endpoint for stateful service does not exist, bail
                    throw new FabricServicePrimaryEndpointNotFoundException(
                        resolvedServicePartition.ServiceName,
                        rspEndpoints);
                }
            }
            this.Log($"Picked resolved service endpoint: Role={targetEndpoint.Role}, Address={targetEndpoint.Address}", LogLevel.Debug);

            // targetEndpoint is still a collection of named endpoints
            this.Log($"Locating endpoint address: ListenerName={serviceEndpoint.ListenerName}", LogLevel.Debug);
            ServiceEndpointCollection serviceEndpointCollection;
            if (!ServiceEndpointCollection.TryParseEndpointsString(targetEndpoint.Address, out serviceEndpointCollection))
            {
                // We expect that the collection is parsable
                throw new FabricServiceEndpointAdressNotParsableException(
                    resolvedServicePartition.ServiceName,
                    targetEndpoint.Address);
            }

            string endpointAddress;
            if (!serviceEndpointCollection.TryGetEndpointAddress(listenerName: serviceEndpoint.ListenerName, endpointAddress: out endpointAddress))
            {
                // The named endpoint could not be found
                throw new FabricServiceNamedEndpointNotFoundException(
                    resolvedServicePartition.ServiceName,
                    serviceEndpoint.ListenerName,
                    targetEndpoint.Address);
            }

            Uri endpointUri;
            if (!Uri.TryCreate(endpointAddress, UriKind.Absolute, out endpointUri))
            {
                throw new FabricServiceInvalidEndpointUriException(
                    resolvedServicePartition.ServiceName,
                    serviceEndpoint.ListenerName,
                    endpointAddress);
            }

            this.Log($"Located endpoint URI is '{endpointUri}'", LogLevel.Debug);
            return endpointUri;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Nothing to dispose
        }

        private sealed class FabricServiceRouterActivityType : SingletonActivityType<FabricServiceRouterActivityType>
        {
            public FabricServiceRouterActivityType()
                : base("SP.FabricHttpRouter.GetNextEndpoint")
            {
            }
        }

        private void Log(string msg, LogLevel logLevel = LogLevel.Information)
        {
            switch (logLevel)
            {
                case LogLevel.Error:
                    if (this.logger == null)
                    {
                        ServicePlatformFabricTrace.Instance.TraceError(msg);
                    }
                    else
                    {
                        this.logger.LogError(msg);
                    }

                    break;
                case LogLevel.Debug:
                    if (this.logger == null)
                    {
                        ServicePlatformFabricTrace.Instance.TraceVerbose(msg);
                    }
                    else
                    {
                        this.logger.LogDebug(msg);
                    }

                    break;
                default:
                    if (this.logger == null)
                    {
                        ServicePlatformFabricTrace.Instance.TraceInformation(msg);
                    }
                    else
                    {
                        this.logger.LogInformation(msg);
                    }

                    break;
            }
        }
    }
}
