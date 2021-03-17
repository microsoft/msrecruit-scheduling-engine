//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>
namespace CommonLibrary.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class JobOfferTemplateParticipant
    {
        [DataMember(Name = "Participant")]
        public string Participant { get; set; }

        [DataMember(Name = "OID")]
        public string OID { get; set; }

        [DataMember(Name = "Role")]
        public JobOfferTemplateRole Role { get; set; }

        [DataMember(Name = "CreatedAt")]
        public DateTime CreatedAt { get; set; }

        [DataMember(Name = "UpdatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
