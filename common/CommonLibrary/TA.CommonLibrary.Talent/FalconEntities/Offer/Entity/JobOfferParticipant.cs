//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>

namespace TA.CommonLibrary.Common.Provisioning.Entities.FalconEntities.Offer
{
    using TA.CommonLibrary.Common.DocumentDB.Contracts;
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class JobOfferParticipant : DocDbEntity
    {
        [DataMember(Name = "JobOfferParticipantID")]
        public string JobOfferParticipantID { get; set; }

        [DataMember(Name = "JobOffer")]
        public string JobOffer { get; set; }

        [DataMember(Name = "JobApplicationParticipant")]
        public string JobApplicationParticipant { get; set; }

        [DataMember(Name = "JobApplicationActivityParticipant")]
        public string JobApplicationActivityParticipant { get; set; }

        [DataMember(Name = "OID")]
        public string OID { get; set; }

        [DataMember(Name = "Role")]
        public JobOfferRole Role { get; set; }

        [DataMember(Name = "Approval")]
        public JobOfferApproval Approval { get; set; }

        [DataMember(Name = "Ordinal")]
        public long? JobOfferParticipantOrdinal { get; set; }

        [DataMember(Name = "JobOfferLastEditedOn")]
        public DateTime? JobOfferLastEditedOn { get; set; }
    }
}
