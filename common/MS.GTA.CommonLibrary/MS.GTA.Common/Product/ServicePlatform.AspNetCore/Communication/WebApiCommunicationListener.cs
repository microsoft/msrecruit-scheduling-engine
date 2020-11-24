//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Description;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.AspNetCore.Utils;
using MS.GTA.ServicePlatform.Context;
using MS.GTA.ServicePlatform.Fabric.Extensions;
using MS.GTA.ServicePlatform.Tracing;
using MS.GTA.ServicePlatform.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;

namespace MS.GTA.ServicePlatform.AspNetCore.Communication
{
    using System.Reflection;
    using Builder;
    using Microsoft.AspNetCore.Hosting.Internal;

    /// <summary>
    /// This class provides a Web API Communication Listener and sets the EnvironmentContext with values from Service Fabric.
    /// </summary>
    /// <typeparam name="TStartup">The startup type.</typeparam>
    public abstract class WebApiCommunicationListener<TStartup> : ICommunicationListener where TStartup : class
    {
        private readonly System.Fabric.ServiceContext serviceContext;
        private readonly Context.IEnvironmentContext environmentContext;
        private readonly bool isStateful;
        private readonly string endpointName;
        private readonly string appRoot;
        private string listeningAddress;
        private string baseAddress;
        private ILogger logger;
        private IWebHost webHost;
        private Func<IWebHostBuilder, IWebHostBuilder> webHostBuilder;
        private UniqueServiceUriSegments uniqueUriSegments;

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiCommunicationListener{T}" /> class for a stateful service.
        /// </summary>
        /// <param name="serviceContext">Stateful service context instance.</param>
        /// <param name="endpointName">Endpoint name.</param>
        /// <param name="communicationListenerOptions">Communication listener options.</param>
        internal WebApiCommunicationListener(StatefulServiceContext serviceContext, string endpointName, CommunicationListenerOptions communicationListenerOptions)
            : this(serviceContext, endpointName, communicationListenerOptions, isStateful: true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebApiCommunicationListener{T}" /> class for a stateless service.
        /// </summary>
        /// <param name="serviceContext">Stateless service context instance.</param>
        /// <param name="endpointName">Endpoint name.</param>
        /// <param name="communicationListenerOptions">Communication listener options</param>
        internal WebApiCommunicationListener(StatelessServiceContext serviceContext, string endpointName, CommunicationListenerOptions communicationListenerOptions)
            : this(serviceContext, endpointName, communicationListenerOptions, isStateful: false)
        {
        }

        private WebApiCommunicationListener(System.Fabric.ServiceContext serviceContext, string endpointName, CommunicationListenerOptions communicationListenerOptions, bool isStateful)
        {
            Contract.CheckValue(serviceContext, nameof(serviceContext));
            Contract.CheckValue(endpointName, nameof(endpointName));
            Contract.CheckValue(communicationListenerOptions, nameof(communicationListenerOptions));

            this.webHostBuilder = communicationListenerOptions.WebHostBuilder ?? BuildKestrelWebHost;
            this.serviceContext = serviceContext;
            this.endpointName = endpointName;
            this.appRoot = communicationListenerOptions.AppRoot;
            this.isStateful = isStateful;
            this.uniqueUriSegments = communicationListenerOptions.UniqueUriSegments;

            this.environmentContext = new Context.CloudEnvironment(
                serviceContext.CodePackageActivationContext.ApplicationName,
                serviceContext.ServiceName.ToString(),
                serviceContext.CodePackageActivationContext.CodePackageVersion,
                serviceContext.PartitionId.ToString(),
                serviceContext.ReplicaOrInstanceId.ToString(),
                string.Empty);
        }

        private static IWebHostBuilder BuildKestrelWebHost(IWebHostBuilder builder)
        {
            return builder.UseKestrel();
        }

        /// <inheritdoc />
        /// <summary>
        /// Build web host and start.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task of the listening address string.</returns>
        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            return Context.ServiceContext.Activity.ExecuteRoot(
                CreateRootExecutionContext(),
                OpenWebApiCommunicationListenerActivity.Instance,
                () =>
                {
                    var serviceEndpoint = this.serviceContext.CodePackageActivationContext.GetEndpoint(this.endpointName);
                    var serviceUriPrefix = GetServiceUriPrefix(appRoot, serviceContext, isStateful, uniqueUriSegments);
                    listeningAddress = GetListeningAddress(serviceEndpoint, serviceContext, serviceUriPrefix);
                    baseAddress = GetBaseAddress(serviceEndpoint, serviceContext);

                    IServiceCollection listenerServices = null;
                    this.webHost = webHostBuilder(new WebHostBuilder())
                        .UseUrls(baseAddress)
                        .ConfigureServices(builderServices =>
                        {
                            listenerServices = builderServices;
                            builderServices.AddSingleton(
                                typeof(IStartup),
                                serviceProvider =>
                                {
                                    var env = serviceProvider.GetRequiredService<IHostingEnvironment>();
                                    var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

                                    // Here we load the regular startup methods for TStartup (as UseStartup<TStartup>() normally would)
                                    // but we insert our own Configure and ConfigureServices method into each.
                                    StartupMethods startupMethods = StartupLoader.LoadMethods(serviceProvider, typeof(TStartup), env.EnvironmentName);

                                    return new ConventionBasedStartup(new StartupMethods(
                                        this,
                                        (IApplicationBuilder app) =>
                                        {
                                            app.UsePathBase(serviceUriPrefix);
                                            app.UsePathBaseRouting(listeningAddress, baseAddress);
                                            this.Configure(app, env, loggerFactory);
                                            app.RemovePathBaseRouting(listeningAddress, baseAddress);
                                            startupMethods.ConfigureDelegate.Invoke(app);
                                            logger = loggerFactory.CreateLogger(GetType());
                                        },
                                        (IServiceCollection startupServices) =>
                                        {
                                            this.ConfigureServices(startupServices);
                                            return startupMethods.ConfigureServicesDelegate.Invoke(startupServices);
                                        }));
                                });
                        })
                        .UseSetting(WebHostDefaults.ApplicationKey, typeof(TStartup).GetTypeInfo().Assembly.FullName)
                        .Build();

                    try
                    {
                        AspNetCoreTrace.Instance.TraceInformation($"Starting web server on {listeningAddress}");
                        TraceServerConfiguration(listenerServices);
                        this.webHost.Start();
                    }
                    catch (Exception ex)
                    {
                        AspNetCoreTrace.Instance.TraceError($"Web server failed to open: {ex}");
                        this.StopWebServer();
                        throw;
                    }

                     return Task.FromResult(listeningAddress);
                });
        }

        private static void TraceServerConfiguration(IServiceCollection services)
        {
            Contract.AssertValueOrNull(services, nameof(services));

            // Trace IoC dependencies
            if (services == null)
            {
                AspNetCoreTrace.Instance.TraceWarning($"{TracingConstants.TraceIndent}Dependencies: (collection missing)");
            }
            else
            {
                // Not sorted these need to be traced in the order in which they are configured to diagnose dependency issues
                AspNetCoreTrace.Instance.TraceInformation($"{TracingConstants.TraceIndent}Dependencies:");
                ServiceCollectionUtil.TraceDependencies(
                    AspNetCoreTrace.Instance,
                    services,
                    $"{TracingConstants.TraceIndent}{TracingConstants.TraceIndent}Configured dependency: ");
            }

            // Trace loaded assemblies, ordered alphabetically
            AspNetCoreTrace.Instance.TraceInformation($"{TracingConstants.TraceIndent}Assemblies:");
            AssemblyUtil.TraceLoadedAssemblies(
                AspNetCoreTrace.Instance,
                $"{TracingConstants.TraceIndent}{TracingConstants.TraceIndent}Loaded assembly: ");
        }

        /// <summary>
        /// Close web host connection.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Task after closing connection.</returns>
        public Task CloseAsync(CancellationToken cancellationToken)
        {
            return logger.ExecuteRoot(
                CreateRootExecutionContext(),
                CloseWebApiCommunicationListenerActivity.Instance,
                () =>
                {
                    logger.LogInformation($"Closing web server on {listeningAddress}");
                    this.StopWebServer();
                    return Task.FromResult(true);
                });
        }

        /// <summary>
        /// Stops web server.
        /// </summary>
        public void Abort()
        {
            try
            {
                logger.ExecuteRoot(
                    CreateRootExecutionContext(),
                    AbortWebApiCommunicationListenerActivity.Instance,
                    () =>
                    {
                        logger.LogInformation($"Aborting web server on {listeningAddress}");
                        this.StopWebServer();
                    });
            }
            catch (Exception ex)
            {
                logger?.LogError($"WebApiCommunicationListener encountered an exception while aborting: {ex}");
            }
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container that are shared among all communication listeners.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <seealso cref="http://go.microsoft.com/fwlink/?LinkID=398940"/>
        protected virtual void ConfigureServices(IServiceCollection services)
        {
            Contract.CheckValue(services, nameof(services));

            services.AddServiceFabricConfigurationSettings();
            services.AddServiceFabricFeatureSwitchManager();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline in ways that are shared among all communication listeners.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        protected virtual void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Contract.CheckValue(app, nameof(app));
            Contract.CheckValue(env, nameof(env));
            Contract.CheckValue(loggerFactory, nameof(loggerFactory));
        }

        private static string GetServiceUriPrefix(string appRoot, System.Fabric.ServiceContext serviceContext, bool isStateful, UniqueServiceUriSegments uniqueUriSegments)
        {
            var uriSegments = new List<string>();

            if (!string.IsNullOrWhiteSpace(appRoot))
            {
                uriSegments.Add(appRoot.TrimEnd('/'));
            }

            // In order for the Service Fabric Routing middleware to correctly identify the intended service for incoming requests the url must
            // include the service name. The middleware will compare the name of currently executing service to the service name in the url and
            // be able to determine if this requests is intended for itself.
            if (uniqueUriSegments.HasFlag(UniqueServiceUriSegments.ServiceName))
            {
                uriSegments.Add(serviceContext.ServiceName.AbsolutePath);
            }

            if (uniqueUriSegments.HasFlag(UniqueServiceUriSegments.Partition))
            {
                uriSegments.Add(serviceContext.PartitionId.ToString("D"));
            }

            if (uniqueUriSegments.HasFlag(UniqueServiceUriSegments.ReplicaOrInstanceId))
            {
                uriSegments.Add(serviceContext.ReplicaOrInstanceId.ToString(CultureInfo.InvariantCulture));
            }

            // Similar to above, stateful services can have many partitions and replicas and they must be individually addressable. The partition and replicat ids are included in the address.
            // It is possible for there to be two instances of the same replica. If service fabric determines a replica would be healthier on a different partition it can 
            // creates new primary instance of the replica for write operations and demote the existing instance to secondary for read operations.
            // The extra GUID which is added at the end of the listening address is to ensure these different instances of same replica are still unique.
            if (isStateful)
            {
                uriSegments.AddRange(new List<string>() { Guid.NewGuid().ToString("D") });
            }

            return string.Join("/", uriSegments.ToArray());
        }

        private static string GetBaseAddress(EndpointResourceDescription serviceEndpoint, System.Fabric.ServiceContext serviceContext)
        {
            return (new UriBuilder(serviceEndpoint.Protocol.ToString(), serviceContext.NodeContext.IPAddressOrFQDN, serviceEndpoint.Port)).Uri.ToString();
        }

        private static string GetListeningAddress(EndpointResourceDescription serviceEndpoint, System.Fabric.ServiceContext serviceContext, string servicePrefix)
        {
            return (new UriBuilder(serviceEndpoint.Protocol.ToString(), serviceContext.NodeContext.IPAddressOrFQDN, serviceEndpoint.Port, servicePrefix)).Uri.ToString();
        }

        private void StopWebServer()
        {
            if (this.webHost != null)
            {
                this.webHost.Dispose();
                this.webHost = null;
            }
        }

        private Context.RootExecutionContext CreateRootExecutionContext()
        {
            var context = new Context.RootExecutionContext();
            context.EnvironmentContext = this.environmentContext;
            context.SessionId = Guid.NewGuid();
            context.RootActivityId = context.SessionId;
            return context;
        }

        private sealed class OpenWebApiCommunicationListenerActivity : Context.SingletonActivityType<OpenWebApiCommunicationListenerActivity>
        {
            public OpenWebApiCommunicationListenerActivity()
                : base("SP.CommunicationListener.Open")
            {
            }
        }

        private sealed class CloseWebApiCommunicationListenerActivity : Context.SingletonActivityType<CloseWebApiCommunicationListenerActivity>
        {
            public CloseWebApiCommunicationListenerActivity()
                : base("SP.CommunicationListener.Close")
            {
            }
        }

        private sealed class AbortWebApiCommunicationListenerActivity : Context.SingletonActivityType<AbortWebApiCommunicationListenerActivity>
        {
            public AbortWebApiCommunicationListenerActivity()
                : base("SP.CommunicationListener.Abort")
            {
            }
        }
    }
}
