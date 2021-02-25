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
    public class JobOfferTemplateToken
    {
        [DataMember(Name = "TokenId")]
        public string TokenId { get; set; }

        [DataMember(Name = "TokenKey")]
        public string TokenKey { get; set; }

        [DataMember(Name = "InUse")]
        public bool InUse { get; set; }
    }
}