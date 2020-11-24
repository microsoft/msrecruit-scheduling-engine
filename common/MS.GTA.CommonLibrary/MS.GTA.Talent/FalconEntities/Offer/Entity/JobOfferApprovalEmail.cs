//----------------------------------------------------------------------------
// <copyright file="JobOfferApprovalEmail.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using MS.GTA.Common.DocumentDB.Contracts;
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
