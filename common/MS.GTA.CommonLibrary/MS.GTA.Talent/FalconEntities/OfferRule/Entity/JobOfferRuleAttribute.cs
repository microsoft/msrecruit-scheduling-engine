// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferRuleAttribute.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.OfferRule
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