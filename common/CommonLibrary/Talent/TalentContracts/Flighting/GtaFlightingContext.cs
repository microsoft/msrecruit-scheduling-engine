//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Talent.TalentContracts.Flighting
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Flight context.
    /// </summary>
    [DataContract]
    public class GtaFlightingContext
    {
        /// <summary>
        /// Gets or sets name of the context.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets value of the context.
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets value of the context type.
        /// </summary>
        [DataMember(Name = "contextType")]
        public FlightingContextType ContextType { get; set; }
    }
}
