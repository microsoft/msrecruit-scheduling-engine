// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferRule.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.OfferRule
{
    using MS.GTA.Common.DocumentDB.Contracts;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    [DataContract]
    public class JobOfferRule : DocDbEntity
    {
        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "TokenId")]
        public string TokenId { get; set; }

        [DataMember(Name = "Attributes")]
        public IList<JobOfferRuleAttribute> Attributes { get; set; }

        [DataMember(Name = "Status")]
        public JobOfferRuleStatus Status { get; set; }

        [DataMember(Name = "Versions")]
        public IList<JobOfferRuleVersion> Versions { get; set; }
    }
}