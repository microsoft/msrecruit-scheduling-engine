//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Routing.Contracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The HCM cluster to BAP location mapping information
    /// </summary>
    [DataContract]
    public class HCMClusterToBAPLocationMap
    {
        /// <summary>
        /// Gets or sets the azure regions for the BAP location
        /// </summary>
        [DataMember(Name = "regions")]
        public IEnumerable<string> Regions { get; set; }

        /// <summary>
        /// Gets or sets the BAP location
        /// </summary>
        [DataMember(Name = "bapLocation")]
        public string BapLocation { get; set; }

        /// <summary>
        /// Gets or sets the enabled state for the BAP location. If true, then users can be routed to that region
        /// </summary>
        [DataMember(Name = "enabled")]
        public bool Enabled { get; set; }
    }
}
