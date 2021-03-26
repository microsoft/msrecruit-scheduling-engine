//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Get the Template Metadata Request
    /// </summary>
    [DataContract]
    public class TemplatePackageMetadataRequest
    {
        /// <summary>
        /// Gets or sets the collection of filter by statuses
        /// </summary>
        [DataMember(Name = "filterBy", IsRequired = false, EmitDefaultValue = false)]
        public IList<DashboardFilterBy> FilterBy { get; set; }

        /// <summary>
        /// Gets or sets the collection of sorted column
        /// </summary>
        [DataMember(Name = "sortedBy", IsRequired = false, EmitDefaultValue = false)]
        public DashboardSortedBy? SortedBy { get; set; }

        /// <summary>
        /// Gets or sets skip
        /// </summary>
        [DataMember(Name = "skip", IsRequired = false, EmitDefaultValue = false)]
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets Take
        /// </summary>
        [DataMember(Name = "take", IsRequired = false, EmitDefaultValue = false)]
        public int Take { get; set; }

        /// <summary>
        /// Gets or sets search text
        /// </summary>
        [DataMember(Name = "searchText", IsRequired = false, EmitDefaultValue = false)]
        public string SearchText { get; set; }
    }
}
