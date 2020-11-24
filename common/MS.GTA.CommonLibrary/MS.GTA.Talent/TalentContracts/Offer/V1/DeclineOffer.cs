﻿// <copyright file="DeclineOffer.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;
    using MS.GTA.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// Specifies the Data contract for Decline offer
    /// </summary>
    [DataContract]
    public class DeclineOffer
    {
        /// <summary>
        /// Gets or sets decline reasons.
        /// </summary>
        [DataMember(Name = "reasons", IsRequired = true, EmitDefaultValue = false)]
        public OfferDeclineReason[] Reasons { get; set; }

        /// <summary>
        /// Gets or sets decline comment.
        /// </summary>
        [DataMember(Name = "comment", IsRequired = false, EmitDefaultValue = false)]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to be able to contact the candidate.
        /// </summary>
        [DataMember(Name = "isOkToContactCandidate", IsRequired = false, EmitDefaultValue = false)]
        public bool IsOkToContactCandidate { get; set; }
    }
}
