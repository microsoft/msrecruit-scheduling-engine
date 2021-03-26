//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>

namespace HR.TA..Common.Provisioning.Entities.FalconEntities.Offer
{
    using HR.TA..Common.DocumentDB.Contracts;
    using System.Runtime.Serialization;

    [DataContract]
    public class JobOfferApprovalEmail : DocDbEntity
    {
        [DataMember(Name = "JobOfferID")]
        public string JobOfferID { get; set; }

        [DataMember(Name = "EmailSubject")]
        public string EmailSubject { get; set; }

        [DataMember(Name = "EmailContent")]
        public string EmailContent { get; set; }

        [DataMember(Name = "EmailCC")]
        public string EmailCC { get; set; }
    }
}
