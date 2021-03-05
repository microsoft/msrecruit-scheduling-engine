//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommonDataService.Common.Internal;
using ServicePlatform.Context;

namespace ServicePlatform.Flighting
{
    /// <summary>
    /// Base class for all flights.
    /// </summary>
    /// <remarks>
    /// Defines the flight name to avoid magic strings in other areas of the code.
    /// WhenAdded is the date when the developer added the flight to the source code
    /// and is used by the process to detect old flights which should be removed from code.
    /// Developers should remove any flight older than six months.
    /// </remarks>
    [ImmutableObject(true)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public abstract class Flight
    {
        // Constructor must stay internal and consumers should only use SingletonFlight

        /// <summary>Initializes a new instance of the Flight class.</summary>
        /// <param name="name">Flight name.</param>
        /// <param name="whenAdded">Date when developer added this flight to the code.</param>
        internal Flight(string name, string whenAdded)
        {
            Contract.CheckNonEmpty(name, nameof(name));            

            Name = ConvertToFlightName(name);
            WhenAdded = whenAdded;
        }

        public string Name { get; }

        public string WhenAdded { get; }

        public override bool Equals(object other)
        {
            var otherAsFlight = other as Flight;
            if (otherAsFlight == null)
                return false;

            return Name.Equals(otherAsFlight.Name, StringComparison.Ordinal);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        /// <summary>
        /// Converts a collection of flights to a list of flight names; returns null when flights is null or empty.
        /// </summary>
        internal static List<string> ConvertFlightsToFlightNames(ICollection<Flight> flights)
        {
            if (flights != null && flights.Count > 0)
            {
                return flights.Select(f => f.Name).ToList();
            }

            return null;
        }

        /// <summary>
        /// Converts flight names to Flight instances of type UnknownFlight.
        /// </summary>
        internal static IEnumerable<Flight> ConvertFlightNamesToFlights(IEnumerable<string> names)
        {
            return names != null ?
                names.Select(n => GetFlightInstance(n)) :
                Enumerable.Empty<Flight>();
        }

        internal static Flight GetFlightInstance(string name)
        {
            return new UnknownFlight(name);
        }

        internal static string ConvertToFlightName(string name)
        {
            // Enable string comparisons to be pointer comparisons
            return string.Intern(name);
        }
    }

    /// <summary>
    /// Internal usage of flights.  This allows callers to manage flights as strings.
    /// </summary>
    internal class RuntimeFlight : Flight
    {
        internal RuntimeFlight(string name, string whenAdded = "")
            : base(name, whenAdded)
        {
        }
    }

    /// <summary>
    /// Base class for all singleton flights.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public abstract class SingletonFlight<T> : Flight
        where T : SingletonFlight<T>, new()
    {
        private static readonly T instance = new T();

        protected SingletonFlight(string name, string whenAdded)
            : base(name, whenAdded)
        {
            Contract.Check(instance == null, "Only use of the singleton instance is allowed");
        }

        public static T Instance
        {
            get { return instance; }
        }

        public static bool IsEnabled
        {
            get
            {
                return ServiceContext.Flights.IsEnabled(Instance);
            }
        }
    }

    /// <summary>
    /// Provides a way to create a Flight without knowing the SingletonFlight type.
    /// For internal use only by ServicePlatform.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    internal sealed class UnknownFlight : Flight
    {
        public UnknownFlight(string name)
            : base(name, string.Empty)
        {
        }
    }
}
