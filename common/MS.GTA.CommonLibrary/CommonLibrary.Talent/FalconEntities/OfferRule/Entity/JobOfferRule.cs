//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Provisioning.Entities.FalconEntities.OfferRule
{
    using CommonLibrary.Common.DocumentDB.Contracts;
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
