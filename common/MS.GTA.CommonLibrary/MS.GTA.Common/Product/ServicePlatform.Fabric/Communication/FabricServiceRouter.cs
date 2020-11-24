//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Fabric.Communication.Internal;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.Extensions.Logging;
using System;


namespace MS.GTA.ServicePlatform.Communication.Http.Routers
{
    /// <summary>
    /// Router targeting Fabric HTTP services.
    /// </summary>
    public sealed class FabricServiceRouter : RetriableRouter
    {
        private readonly IServicePartitionResolver servicePartitionResolver;
        private readonly FabricServiceEndpoint serviceEndpoint;
        private readonly FabricServiceRouterOptions fabricRouterOptions;
        private readonly ILoggerFactory loggerFactory;

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceRouter"/> with default <see cref="FabricServiceRouterOptions"/> and <see cref="RequestRetryOptions"/>.
        /// </summary>
        /// <param name="servicePartitionResolver">The <see cref="IServicePartitionResolver"/> to use for service partition resolution. Must not be null.</param>
        /// <param name="serviceEndpoint">The Fabric service endpoint. Must not be null.</param>
        [Obsolete("Please use the constructor with the logger factory.")]
        public FabricServiceRouter(
            IServicePartitionResolver servicePartitionResolver,
            FabricServiceEndpoint serviceEndpoint)
            : this(
                  servicePartitionResolver,
                  serviceEndpoint,
                  new FabricServiceRouterOptions())
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceRouter"/> with default <see cref="FabricServiceRouterOptions"/>.
        /// </summary>
        /// <param name="servicePartitionResolver">The <see cref="IServicePartitionResolver"/> to use for service partition resolution. Must not be null.</param>
        /// <param name="serviceEndpoint">The Fabric service endpoint. Must not be null.</param>
        /// <param name="requestRetryOptions">Request retry options to use.</param>
        [Obsolete("Please use the constructor with the logger factory.")]
        public FabricServiceRouter(
            IServicePartitionResolver servicePartitionResolver,
            FabricServiceEndpoint serviceEndpoint,
            RequestRetryOptions requestRetryOptions)
            : this(
                  servicePartitionResolver,
                  serviceEndpoint,
                  new FabricServiceRouterOptions(),
                  requestRetryOptions)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceRouter"/> with default <see cref="RequestRetryOptions"/>.
        /// </summary>
        /// <param name="servicePartitionResolver">The <see cref="IServicePartitionResolver"/> to use for service partition resolution. Must not be null.</param>
        /// <param name="serviceEndpoint">The Fabric service endpoint. Must not be null.</param>
        /// <param name="fabricRouterOptions">Custom options to alter fabric router behavior.</param>
        [Obsolete("Please use the constructor with the logger factory.")]
        public FabricServiceRouter(
            IServicePartitionResolver servicePartitionResolver,
            FabricServiceEndpoint serviceEndpoint,
            FabricServiceRouterOptions fabricRouterOptions)
        {
            Contract.CheckValue(servicePartitionResolver, nameof(servicePartitionResolver));
            Contract.CheckValue(serviceEndpoint, nameof(serviceEndpoint));
            Contract.CheckValue(fabricRouterOptions, nameof(fabricRouterOptions));

            this.servicePartitionResolver = servicePartitionResolver;
            this.serviceEndpoint = serviceEndpoint;
            this.fabricRouterOptions = new FabricServiceRouterOptions(fabricRouterOptions);
        }

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceRouter"/> with the provided configuration.
        /// </summary>
        /// <param name="servicePartitionResolver">The <see cref="IServicePartitionResolver"/> to use for service partition resolution. Must not be null.</param>
        /// <param name="serviceEndpoint">The Fabric service endpoint. Must not be null.</param>
        /// <param name="fabricRouterOptions">Custom options to alter fabric router behavior.</param>
        /// <param name="requestRetryOptions">Request retry options to use.</param>
        [Obsolete("Please use the constructor with the logger factory.")]
        public FabricServiceRouter(
            IServicePartitionResolver servicePartitionResolver,
            FabricServiceEndpoint serviceEndpoint,
            FabricServiceRouterOptions fabricRouterOptions,
            RequestRetryOptions requestRetryOptions)
            : base(requestRetryOptions)
        {
            Contract.CheckValue(servicePartitionResolver, nameof(servicePartitionResolver));
            Contract.CheckValue(serviceEndpoint, nameof(serviceEndpoint));
            Contract.CheckValue(fabricRouterOptions, nameof(fabricRouterOptions));

            this.servicePartitionResolver = servicePartitionResolver;
            this.serviceEndpoint = serviceEndpoint;
            this.fabricRouterOptions = new FabricServiceRouterOptions(fabricRouterOptions);
        }

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceRouter"/> with default <see cref="FabricServiceRouterOptions"/> and <see cref="RequestRetryOptions"/>.
        /// </summary>
        /// <param name="servicePartitionResolver">The <see cref="IServicePartitionResolver"/> to use for service partition resolution. Must not be null.</param>
        /// <param name="serviceEndpoint">The Fabric service endpoint. Must not be null.</param>
        /// <param name="loggerFactory">The instnce for <see cref="ILoggerFactory"/>.</param>
        public FabricServiceRouter(
            IServicePartitionResolver servicePartitionResolver,
            FabricServiceEndpoint serviceEndpoint,
            ILoggerFactory loggerFactory)
            : this(
                  servicePartitionResolver,
                  serviceEndpoint,
                  new FabricServiceRouterOptions(),
                  loggerFactory)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceRouter"/> with default <see cref="FabricServiceRouterOptions"/>.
        /// </summary>
        /// <param name="servicePartitionResolver">The <see cref="IServicePartitionResolver"/> to use for service partition resolution. Must not be null.</param>
        /// <param name="serviceEndpoint">The Fabric service endpoint. Must not be null.</param>
        /// <param name="requestRetryOptions">Request retry options to use.</param>
        /// <param name="loggerFactory">The instnce for <see cref="ILoggerFactory"/>.</param>
        public FabricServiceRouter(
            IServicePartitionResolver servicePartitionResolver,
            FabricServiceEndpoint serviceEndpoint,
            RequestRetryOptions requestRetryOptions,
            ILoggerFactory loggerFactory)
            : this(
                  servicePartitionResolver,
                  serviceEndpoint,
                  new FabricServiceRouterOptions(),
                  requestRetryOptions,
                  loggerFactory)
        {
        }

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceRouter"/> with default <see cref="RequestRetryOptions"/>.
        /// </summary>
        /// <param name="servicePartitionResolver">The <see cref="IServicePartitionResolver"/> to use for service partition resolution. Must not be null.</param>
        /// <param name="serviceEndpoint">The Fabric service endpoint. Must not be null.</param>
        /// <param name="fabricRouterOptions">Custom options to alter fabric router behavior.</param>
        /// <param name="loggerFactory">The instnce for <see cref="ILoggerFactory"/>.</param>
        public FabricServiceRouter(
            IServicePartitionResolver servicePartitionResolver,
            FabricServiceEndpoint serviceEndpoint,
            FabricServiceRouterOptions fabricRouterOptions,
            ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(servicePartitionResolver, nameof(servicePartitionResolver));
            Contract.CheckValue(serviceEndpoint, nameof(serviceEndpoint));
            Contract.CheckValue(fabricRouterOptions, nameof(fabricRouterOptions));

            this.servicePartitionResolver = servicePartitionResolver;
            this.serviceEndpoint = serviceEndpoint;
            this.fabricRouterOptions = new FabricServiceRouterOptions(fabricRouterOptions);
            this.loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Creates a new instance of <see cref="FabricServiceRouter"/> with the provided configuration.
        /// </summary>
        /// <param name="servicePartitionResolver">The <see cref="IServicePartitionResolver"/> to use for service partition resolution. Must not be null.</param>
        /// <param name="serviceEndpoint">The Fabric service endpoint. Must not be null.</param>
        /// <param name="fabricRouterOptions">Custom options to alter fabric router behavior.</param>
        /// <param name="requestRetryOptions">Request retry options to use.</param>
        /// <param name="loggerFactory">The instance for <see cref="ILoggerFactory"/>.</param>
        public FabricServiceRouter(
            IServicePartitionResolver servicePartitionResolver,
            FabricServiceEndpoint serviceEndpoint,
            FabricServiceRouterOptions fabricRouterOptions,
            RequestRetryOptions requestRetryOptions,
            ILoggerFactory loggerFactory)
            : base(requestRetryOptions)
        {
            Contract.CheckValue(servicePartitionResolver, nameof(servicePartitionResolver));
            Contract.CheckValue(serviceEndpoint, nameof(serviceEndpoint));
            Contract.CheckValue(fabricRouterOptions, nameof(fabricRouterOptions));

            this.servicePartitionResolver = servicePartitionResolver;
            this.serviceEndpoint = serviceEndpoint;
            this.fabricRouterOptions = new FabricServiceRouterOptions(fabricRouterOptions);
            this.loggerFactory = loggerFactory;
        }

        /// <inheritdoc />
        protected override IRetriableRouterRequest CreateRetriableRouterRequest()
        {
            return new FabricServiceRouterRequest(
                servicePartitionResolver,
                serviceEndpoint,
                fabricRouterOptions,
                this.loggerFactory);
        }
    }
}
