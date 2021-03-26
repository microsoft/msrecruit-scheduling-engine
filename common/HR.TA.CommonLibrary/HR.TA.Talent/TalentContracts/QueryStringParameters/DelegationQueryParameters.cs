//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.TalentContracts.QueryStringParameters
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The delegation query parameters.
    /// </summary>
    [DataContract]
    public class DelegationQueryParameters : PaginationAndSortQueryParameters
    {
        /// <summary>
        /// Gets or sets whether to include worker information.
        /// </summary>
        [DataMember(Name = "includeWorker", IsRequired = false)]
        public bool IncludeWorker { get; set; } = false;

        /// <summary>
        /// Gets or sets To email of delegation request for search.
        /// </summary>
        [DataMember(Name = "searchToEmail", IsRequired = false)]
        public string SearchToEmail { get; set; }

        /// <summary>
        /// Gets or sets From email of delegation request for search.
        /// </summary>
        [DataMember(Name = "searchFromEmail", IsRequired = false)]
        public string SearchFromEmail { get; set; }

    }
}
