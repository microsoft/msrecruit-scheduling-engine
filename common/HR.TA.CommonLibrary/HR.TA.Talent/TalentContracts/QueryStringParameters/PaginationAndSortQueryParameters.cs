//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.TalentContracts.QueryStringParameters
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The pagination and sorting query parameters.
    /// </summary>
    [DataContract]
    public class PaginationAndSortQueryParameters
    {
        /// <summary>
        /// Gets or sets page number
        /// </summary>
        [DataMember(Name = "pageNumber", IsRequired = false, EmitDefaultValue = false)]
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or sets page size
        /// </summary>
        [DataMember(Name = "pageSize", IsRequired = false, EmitDefaultValue = false)]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the sort by field.
        /// </summary>
        [DataMember(Name = "sortBy", IsRequired = false)]
        public string SortBy { get; set; }

        /// <summary>
        /// Gets or sets the sort direction.
        /// </summary>
        [DataMember(Name = "sortDirection", IsRequired = false)]
        public string SortDirection { get; set; } = "desc";
    }
}
