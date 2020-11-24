//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.AspNetCore.Http.Abstractions;
using MS.GTA.ServicePlatform.Context;
using MS.GTA.ServicePlatform.Flighting;
using MS.GTA.ServicePlatform.Tracing;
using MS.GTA.ServicePlatform.Utils;
using System.Collections.Generic;

namespace MS.GTA.ServicePlatform.AspNetCore.Flighting
{
    /// <summary>
    /// Given an implementation of a flights evaluation context provider it invokes the configured flights
    /// provider and populates the flight context for the request.
    /// </summary>
    internal sealed class FlightsProviderMiddleware<TFlightsProvider, TEvaluationContext> : IStatelessHttpMiddleware
            where TFlightsProvider : IFlightsProvider<TEvaluationContext>
            where TEvaluationContext : class
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Func<HttpContext, TEvaluationContext> evaluationContextProvider;

        public FlightsProviderMiddleware(IServiceProvider serviceProvider, Func<HttpContext, TEvaluationContext> evaluationContextProvider)
        {
            Contract.AssertValue(serviceProvider, nameof(serviceProvider));
            Contract.AssertValue(evaluationContextProvider, nameof(evaluationContextProvider));

            this.serviceProvider = serviceProvider;
            this.evaluationContextProvider = evaluationContextProvider;
        }

        public async Task InvokeAsync(HttpContext httpContext, Func<Task> next)
        {
            IReadOnlyCollection<Flight> enabledFlights = null;
            ServiceContext.Activity.Execute(
                FlightsEvaluationPipelineActivity.Instance,
                () =>
                {
                    var flightsProvider = serviceProvider.GetService<TFlightsProvider>();
                    if (flightsProvider == null)
                        throw new FlightsProviderMissingException(typeof(TFlightsProvider));

                    var enabledFlightsEvaluationContext = evaluationContextProvider(httpContext);
                    enabledFlights = 
                        enabledFlightsEvaluationContext != null ?
                        flightsProvider.GetEnabled(enabledFlightsEvaluationContext) :
                        flightsProvider.GetEnabled();

                    AspNetCoreTrace.Instance.TraceInformation(String.Format(
                        "Request will be in the following flights: {0}",
                        enabledFlights != null ? string.Join(", ", enabledFlights.Select(f => f.Name)) : null));
                });

            using (ServiceContext.Flights.SetEnabledFlights(enabledFlights))
            {
                await next();
            }
        }

        private sealed class FlightsEvaluationPipelineActivity : SingletonActivityType<FlightsEvaluationPipelineActivity>
        {
            public FlightsEvaluationPipelineActivity()
                : base("SP.FlightsEvaluation")
            {
            }
        }
    }
}
