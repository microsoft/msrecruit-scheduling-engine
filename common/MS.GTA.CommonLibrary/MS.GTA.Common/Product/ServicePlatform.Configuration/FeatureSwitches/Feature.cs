//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MS.GTA.CommonDataService.Common.Internal;

namespace MS.GTA.ServicePlatform.Configuration.FeatureSwitches
{
    /// <summary>
    /// Base class for all features.
    /// </summary>
    /// <remarks>
    /// Defines the feature name to avoid magic strings in other areas of the code.
    /// WhenAdded is the date when the developer added the feature to the source code
    /// and is used by the process to detect old features which should be removed from code.
    /// Developers should remove any feature older than six months.
    /// </remarks>
    [ImmutableObject(true)]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public abstract class Feature
    {
        /// <summary>
        /// Initializes a new instance of the Feature class.
        /// </summary>
        /// <param name="name">Feature name.</param>
        /// <param name="whenAdded">Date when developer added this feature to the code.</param>
        internal Feature(string name, string whenAdded)
        {
            Contract.CheckNonEmpty(name, nameof(name));

            Name = name;
        }

        public string Name { get; }

        public string WhenAdded { get; }
    }

    /// <summary>
    /// Base class for all singleton features. This is the only representation supported externally today.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public abstract class SingletonFeature<T> : Feature
        where T : SingletonFeature<T>, new()
    {
        private static readonly T instance = new T();

        protected SingletonFeature(string name, string whenAdded)
            : base(name, whenAdded)
        {
            Contract.Check(instance == null, "Only use of the singleton instance is allowed");
        }

        public static T Instance
        {
            get { return instance; }
        }
    }
}
