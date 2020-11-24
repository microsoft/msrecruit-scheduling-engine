//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class JobOfferApproval
    {
        [DataMember(Name = "Status")]
        public JobOfferApprovalStatus Status { get; set; }

        [DataMember(Name = "StatusReason")]
        public JobOfferApprovalStatusReason StatusReason { get; set; }

        [DataMember(Name = "Comment")]
        public string Comment { get; set; }

        [DataMember(Name = "RequestDate")]
        public DateTime? RequestDate { get; set; }

        [DataMember(Name = "RespondDate")]
        public DateTime? RespondDate { get; set; }

        [DataMember(Name = "IsSubmitted")]
        public bool? IsSubmitted { get; set; }

        [DataMember(Name = "SubmittedBy")]
        public string SubmittedBy { get; set; }
    }
}
