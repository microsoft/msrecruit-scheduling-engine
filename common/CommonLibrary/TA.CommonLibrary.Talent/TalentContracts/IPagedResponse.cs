//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using TA.CommonLibrary.Common.Contracts;
using System.Collections.Generic;

namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    /// <summary>
    /// Interface paged response.
    /// </summary>
    public interface IPagedResponse<T> where T : TalentBaseContract
    {
        /// <summary>
        /// Gets or sets the items.
        /// </summary>
        IEnumerable<T> Items { get; set; }
        /// <summary>
        /// Gets or sets the next page.
        /// </summary>
        string NextPage { get; set; }
        /// <summary>
        /// Gets or sets the page number.
        /// </summary>
        int? PageNumber { get; set; }
        /// <summary>
        /// Gets or sets the page size.
        /// </summary>
        int? PageSize { get; set; }
        /// <summary>
        /// Gets or sets the previous page.
        /// </summary>
        string PreviousPage { get; set; }
        /// <summary>
        /// Gets or sets the total record count.
        /// </summary>
        int? TotalCount { get; set; }
    }
}
