//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Fabric;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using TA.CommonLibrary.ServicePlatform.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace TA.CommonLibrary.ServicePlatform.AspNetCore.Communication
{
    /// <summary>
    /// This class provides an External Web API Communication Listener and sets the EnvironmentContext with values from Service Fabric.
    /// The web host is already configured with ConfigurationSettings, FeatureSwitchManager, and an ExternalWebApiHost.
    /// </summary>
    /// <typeparam name="TStartup">The startup type.</typeparam>
    public class ExternalCommunicationListener<TStartup> : WebApiCommunicationListener<TStartup> where TStartup : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalCommunicationListener{TStartup}" /> class for a stateful service.
        /// </summary>
        /// <param name="serviceContext">Stateful service context instance.</param>
        /// <param name="endpointName">Endpoint name.</param>
        /// <param name="communicationListenerOptions">Options for configuring communication listener.</param>
        public ExternalCommunicationListener(StatefulServiceContext serviceContext, string endpointName, CommunicationListenerOptions communicationListenerOptions)
            : base(serviceContext, endpointName, communicationListenerOptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalCommunicationListener{TStartup}" /> class for a stateless service.
        /// </summary>
        /// <param name="serviceContext">Stateless service context instance.</param>
        /// <param name="endpointName">Endpoint name.</param>
        /// <param name="communicationListenerOptions">Options for configuring communication listener.</param>
        public ExternalCommunicationListener(StatelessServiceContext serviceContext, string endpointName, CommunicationListenerOptions communicationListenerOptions)
            : base(serviceContext, endpointName, communicationListenerOptions)
        {
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <param name="env">The hosting environment.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        protected override void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            base.Configure(app, env, loggerFactory);
            app.UseExternalWebApiHost(Context.ServiceContext.Environment.Current);
        }
    }
}
