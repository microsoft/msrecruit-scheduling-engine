﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The paged response class.
    /// </summary>
    [DataContract]
    public class DelegationRequestPagedResponse : IPagedResponse<DelegationRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DelegationRequestPagedResponse"/> class.
        /// </summary>
        public DelegationRequestPagedResponse()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegationRequestPagedResponse"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public DelegationRequestPagedResponse(IEnumerable<DelegationRequest> data)
        {
            this.Items = data;
        }

        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        [DataMember(Name = "items", IsRequired = false)]
        public IEnumerable<DelegationRequest> Items { get; set; }

        /// <summary>
        /// Gets or sets the next page.
        /// </summary>
        [DataMember(Name = "nextPage", IsRequired = false)]
        public string NextPage { get; set; }

        /// <summary>
        /// Gets or sets the previous page.
        /// </summary>
        [DataMember(Name = "previousPage", IsRequired = false)]
        public string PreviousPage { get; set; }

        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        [DataMember(Name = "pageNumber", IsRequired = false)]
        public int? PageNumber { get; set; }

        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        [DataMember(Name = "pageSize", IsRequired = false)]
        public int? PageSize { get; set; }

        /// <summary>
        /// Gets or sets the total record count.
        /// </summary>
        [DataMember(Name = "totalCount", IsRequired = false)]
        public int? TotalCount { get; set; }
    }
}
