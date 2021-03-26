//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Routing.Contracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The HCM cluster to BAP location mappings collection
    /// </summary>
    [DataContract]
    public class HCMClusterToBAPLocationMapCollection
    {
        /// <summary>
        /// Gets or sets the id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the HCM cluster to BAP locations mappings
        /// </summary>
        [DataMember(Name = "mappings")]
        public IEnumerable<HCMClusterToBAPLocationMap> Mappings { get; set; }
    }
}
