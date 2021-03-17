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
    using CommonLibrary.Common.DocumentDB.Contracts;

    [DataContract]
    public class JobOfferTemplatePackageDetail : DocDbEntity
    {
        [DataMember(Name = "JobOfferTemplatePackageID")]
        public string JobOfferTemplatePackageID { get; set; }

        [DataMember(Name = "JobOfferTemplateID")]
        public string JobOfferTemplateID { get; set; }

        [DataMember(Name = "OriginalTemplatePackageID")]
        public string OriginalTemplatePackageID { get; set; }

        [DataMember(Name = "OriginalTemplateID")]
        public string OriginalTemplateID { get; set; }

        [DataMember(Name = "IsTemplateRequired")]
        public bool IsTemplateRequired { get; set; }   
        
        [DataMember(Name = "Ordinal")]
        public int Ordinal { get; set; }
    }
}
