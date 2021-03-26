//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using HR.TA.ServicePlatform.Privacy;

namespace HR.TA.ServicePlatform.Exceptions
{
    /// <summary>
    /// Indicates that a property should be surfaced as <see cref="CustomData"/> for tracing and (optionally) serialization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ExceptionCustomDataAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets an override for the property name when serializing.
        /// </summary>
        public string Name { get; set; } = null;

        /// <summary>
        /// Gets or sets a value that determines if a property should be serialized between services.
        /// </summary>
        public bool Serialize { get; set; } = false;

        /// <summary>
        /// Gets or sets the PrivacyMarker used to mark values when tracing.
        /// </summary>
        public PrivacyLevel PrivacyLevel { get; set; } = PrivacyLevel.PublicData;
    }
}
