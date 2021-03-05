//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using CommonDataService.Common.Internal;
using ServicePlatform.Flighting;

namespace ServicePlatform.Context
{
    public sealed partial class ServiceContext
    {
        public sealed partial class Flights
        {
            /// <summary>
            /// Sets the enabled flights on the context.
            /// </summary>
            /// <returns>The disposable for the new ServiceContext.</returns>
            public static IDisposable SetEnabledFlights(IEnumerable<Flight> enabledFlights)
            {
                var currentFlights = Current;
                Contract.Check(currentFlights == null, "Once set, the flights cannot be changed");
                Contract.CheckValue(enabledFlights, nameof(enabledFlights));

                return Push(new Flights(enabledFlights));
            }

            public static bool IsEnabled(string flightName)
            {
                Contract.CheckValue(flightName, nameof(flightName));
                return IsEnabled(new RuntimeFlight(flightName));
            }

            internal static Flights Current
            {
                get
                {
                    var currentContext = CurrentContext;
                    if (currentContext == null)
                        return null;

                    return currentContext.flightsContext;
                }
            }

            internal static bool IsEnabled(Flight flight)
            {
                Contract.AssertValue(flight, nameof(flight));

                var currentFlightContext = Current;
                if (currentFlightContext != null)
                    return currentFlightContext.enabledFlights.Contains(flight);

                return false;
            }
        }

        // Instance implementation
        [ImmutableObject(true)]
        public sealed partial class Flights
        {
            private readonly HashSet<Flight> enabledFlights;

            internal Flights(IEnumerable<Flight> enabledFlights)
            {
                Contract.AssertValue(enabledFlights, nameof(enabledFlights));

                this.enabledFlights = new HashSet<Flight>(enabledFlights);
            }

            internal Flights(HashSet<Flight> enabledFlights)
            {
                Contract.AssertValue(enabledFlights, nameof(enabledFlights));

                this.enabledFlights = enabledFlights;
            }

            internal HashSet<Flight> EnabledFlights
            {
                get { return enabledFlights; }
            }
        }
    }
}
