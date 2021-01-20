﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOpeningPositionRequest.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Job Opening Position Request
    /// </summary>
    [DataContract]
    public class JobOpeningPositionRequest
    {
        /// <summary>
        /// Gets or sets skip
        /// </summary>
        [DataMember(Name = "skip", IsRequired = false, EmitDefaultValue = false)]
        public int Skip { get; set; }

        /// <summary>
        /// Gets or sets skip
        /// </summary>
        [DataMember(Name = "take", IsRequired = false, EmitDefaultValue = false)]
        public int Take { get; set; }

        /// <summary>
        /// Gets or sets search text
        /// </summary>
        [DataMember(Name = "searchText", IsRequired = false, EmitDefaultValue = false)]
        public string SearchText { get; set; }

        /// <summary>
        /// Gets or sets search text
        /// </summary>
        [DataMember(Name = "continuationToken", IsRequired = false, EmitDefaultValue = false)]
        public string ContinuationToken { get; set; }
    }
}
