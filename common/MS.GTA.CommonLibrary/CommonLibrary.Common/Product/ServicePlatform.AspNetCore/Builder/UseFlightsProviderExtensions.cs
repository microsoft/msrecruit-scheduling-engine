//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using CommonLibrary.CommonDataService.Common.Internal;
using CommonLibrary.ServicePlatform.AspNetCore.Flighting;
using CommonLibrary.ServicePlatform.Flighting;

namespace CommonLibrary.ServicePlatform.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for integrating flight providers into the ASP.NET Core request execution pipeline.
    /// </summary>
    public static class UseFlightsProviderExtensions
    {
        /// <summary>
        /// Adds a flights provider type into the ASP.NET Core request execution pipeline, taking a <c>Func{HttpContext, TEvaluationContext} 
        /// factory to provide the flight evaluation context for each request.</c>
        /// </summary>
        public static IApplicationBuilder UseFlightsProvider<TFlightsProvider, TEvaluationContext>(this IApplicationBuilder builder, Func<HttpContext, TEvaluationContext> evaluationContextProvider)
            where TFlightsProvider : IFlightsProvider<TEvaluationContext>
            where TEvaluationContext : class
        {
            Contract.CheckValue(builder, nameof(builder));
            Contract.CheckValue(evaluationContextProvider, nameof(evaluationContextProvider));

            // Validate that the flights provider is registered
            var flightsProvider = builder.ApplicationServices.GetService(typeof(TFlightsProvider));
            if (flightsProvider == null)
                throw new FlightsProviderMissingException(typeof(TFlightsProvider));

            var flightsProviderMiddleware = new FlightsProviderMiddleware<TFlightsProvider, TEvaluationContext>(
                builder.ApplicationServices, 
                evaluationContextProvider);

            builder.UseStatelessHttpMiddleware(flightsProviderMiddleware);

            return builder;
        }
    }
}