//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>
namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public class JobOfferTokenValue
    {
        [DataMember(Name = "TokenId")]
        public string TokenId { get; set; }

        [DataMember(Name = "TokenKey")]
        public string TokenKey { get; set; }

        [DataMember(Name = "TokenValue")]
        public string TokenValue { get; set; }

        [DataMember(Name = "DisplayText")]
        public string DisplayText { get; set; }

        [DataMember(Name = "isOverridden")]
        public bool IsOverridden { get; set; }

        [DataMember(Name = "isSelectedExplicitly")]
        public bool IsSelectedExplicitly { get; set; }

        [DataMember(Name = "defaultValue")]
        public string DefaultValue { get; set; }

        [DataMember(Name = "IsOptional")]
        public bool? IsOptional { get; set; }
    }
}
