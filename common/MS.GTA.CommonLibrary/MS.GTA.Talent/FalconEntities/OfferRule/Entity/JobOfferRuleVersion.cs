// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferRuleVersion.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.OfferRule
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class JobOfferRuleVersion
    {
        [DataMember(Name = "VersionId")]
        public string VersionId { get; set; }

        [DataMember(Name = "State")]
        public JobOfferRuleProcessingStatus ProcessingStatus { get; set; }

        [DataMember(Name = "ModifiedBy")]
        public string ModifiedBy { get; set; }

        [DataMember(Name = "ModifiedOn")]
        public DateTime ModifiedOn { get; set; }

        [DataMember(Name = "RulesetFile")]
        public string RulesetFile { get; set; }

        [DataMember(Name ="ErrorFile")]
        public string ErrorFile { get; set; }

        [DataMember(Name = "IsLive")]
        public bool IsLive { get; set; }
    }
}