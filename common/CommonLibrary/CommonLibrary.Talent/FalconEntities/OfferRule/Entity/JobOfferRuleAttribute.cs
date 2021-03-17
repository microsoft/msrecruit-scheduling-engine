//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Provisioning.Entities.FalconEntities.OfferRule
{
    using System.Runtime.Serialization;

    [DataContract]
    public class JobOfferRuleAttribute
    {
        [DataMember(Name = "Level")]
        public int Level { get; set; }

        [DataMember(Name="TokenId")]
        public string TokenId { get; set; }
    }
}
