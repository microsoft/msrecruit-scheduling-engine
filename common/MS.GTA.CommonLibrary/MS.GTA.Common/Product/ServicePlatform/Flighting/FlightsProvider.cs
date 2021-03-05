//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CommonDataService.Common.Internal;

namespace ServicePlatform.Flighting
{
    // TODO - 846272 : Remove with https://msazure.visualstudio.com/OneAgile/_workitems/edit/846272
    [Obsolete("These APIs are obsolete and will be removed in a future release. Use IFlightsProvider<TEvaluationContext> instead.")]
    public interface IFlightsProvider
    {
        [Obsolete("This API is obsolete and will be removed in a future release. Use IFlightsProvider<TEvaluationContext> instead.")]
        bool IsEnabled(Flight flight);

        [Obsolete("This API is obsolete and will be removed in a future release. Use IFlightsProvider<TEvaluationContext> instead.")]
        bool IsEnabled<TContext>(Flight flight, TContext context) where TContext : class;

        [Obsolete("This API is obsolete and will be removed in a future release. Use IFlightsProvider<TEvaluationContext> instead.")]
        IReadOnlyCollection<Flight> GetEnabled();

        [Obsolete("This API is obsolete and will be removed in a future release. Use IFlightsProvider<TEvaluationContext> instead.")]
        IReadOnlyCollection<Flight> GetEnabled<TContext>(TContext context) where TContext : class;
    }

    /// <summary>
    /// A service contract for a flights provider.
    /// </summary>
    public interface IFlightsProvider<TEvaluationContext> where TEvaluationContext : class
    {
        /// <summary>
        /// Determines whether the provided flight is universally enabled or not.
        /// </summary>
        bool IsEnabled(Flight flight);

        /// <summary>
        /// Determines whether the provided flight is enabled or not in the given evaluation context.
        /// </summary>
        bool IsEnabled(Flight flight, TEvaluationContext evaluationContext);

        /// <summary>
        /// Gives all universally enabled flights.
        /// </summary>
        IReadOnlyCollection<Flight> GetEnabled();

        /// <summary>
        /// Gives all universally enabled flights in the provided evaluation context.
        /// </summary>
        IReadOnlyCollection<Flight> GetEnabled(TEvaluationContext evaluationContext);
    }

#pragma warning disable 618 // TODO - 846272 : Remove with https://msazure.visualstudio.com/OneAgile/_workitems/edit/846272
    /// <summary>
    /// A base implementation for flights providers that need to return dynamic set of enabled flights.
    /// </summary>
    public abstract class FlightsProvider<TEvaluationContext> : IFlightsProvider<TEvaluationContext>, IFlightsProvider
        where TEvaluationContext : class
    {
        private ConcurrentDictionary<string, Flight> flightsCache = new ConcurrentDictionary<string, Flight>();

        /// <summary>
        /// Implementing class should return all universally enabled flight names.
        /// </summary>
        protected abstract IEnumerable<string> GetEnabledNames();

        /// <summary>
        /// Implementing class should return all enabled flight names in the given evaluation context.
        /// </summary>
        protected abstract IEnumerable<string> GetEnabledNames(TEvaluationContext evaluationContext);

        // TODO - 846272 : Remove with https://msazure.visualstudio.com/OneAgile/_workitems/edit/846272
        [Obsolete("This API is obsolete and will be removed in a future release. Use IFlightsProvider<TEvaluationContext> members instead.")]
        protected abstract IEnumerable<string> GetEnabledNames<TContext>(TContext context) where TContext : class;

        /// <summary>
        /// Implementing class should return whether the provided flight is universally enabled.
        /// </summary>
        public abstract bool IsEnabled(Flight flight);

        /// <summary>
        /// Implementing class should return whether the provided flight is enabled in the given evaluation context.
        /// </summary>
        public abstract bool IsEnabled(Flight flight, TEvaluationContext evaluationContext);

        // TODO - 846272 : Remove with https://msazure.visualstudio.com/OneAgile/_workitems/edit/846272
        [Obsolete("This API is obsolete and will be removed in a future release. Use IFlightsProvider<TEvaluationContext> members instead.")]
        public abstract bool IsEnabled<TContext>(Flight flight, TContext context) where TContext : class;

        /// <summary>
        /// Gives all universally enabled flights.
        /// </summary>
        public IReadOnlyCollection<Flight> GetEnabled()
        {
            var enabledNames = GetEnabledNames();
            return GetOrCreateFlights(enabledNames);
        }

        /// <summary>
        /// Gives all universally enabled flights in the provided evaluation context.
        /// </summary>
        public IReadOnlyCollection<Flight> GetEnabled(TEvaluationContext evaluationContext)
        {
            Contract.CheckValue(evaluationContext, nameof(evaluationContext));

            var enabledNames = GetEnabledNames(evaluationContext);
            return GetOrCreateFlights(enabledNames);
        }

        // TODO - 846272 : Remove with https://msazure.visualstudio.com/OneAgile/_workitems/edit/846272
        [Obsolete("This API is obsolete and will be removed in a future release. Use IFlightsProvider<TEvaluationContext> members instead.")]
        public IReadOnlyCollection<Flight> GetEnabled<TContext>(TContext context) where TContext : class
        {
            Contract.CheckValue(context, nameof(context));

            // This will invoke the legacy protected method
            var enabledNames = GetEnabledNames<TContext>(context);
            return GetOrCreateFlights(enabledNames);
        }

        private IReadOnlyCollection<Flight> GetOrCreateFlights(IEnumerable<string> enabledNames)
        {
            Contract.AssertValue(enabledNames, nameof(enabledNames));

            var enabledList = new List<Flight>();

            foreach (var enabledName in enabledNames)
            {
                // Most of the time the flight will already be in the flightsCache so
                // avoid creating a Fight instance when possible.
                var flightName = Flight.ConvertToFlightName(enabledName);

                enabledList.Add(flightsCache.GetOrAdd(flightName, (key) => Flight.GetFlightInstance(key)));
            }

            return enabledList.AsReadOnly();
        }
    }
#pragma warning restore 618 // TODO - 846272 : Remove with https://msazure.visualstudio.com/OneAgile/_workitems/edit/846272
}
