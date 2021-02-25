//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.OfferRule
{
    using MS.GTA.Common.DocumentDB.Contracts;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class JobOfferRuleParticipant: DocDbEntity
    {
        [DataMember(Name = "Oid")]
        public string OID { get; set; }

        [DataMember(Name = "RulesetId")]
        public string RulesetId { get; set; }

        [DataMember(Name = "Role")]
        public JobOfferRuleParticipantRole Role { get; set; }
    }
}
