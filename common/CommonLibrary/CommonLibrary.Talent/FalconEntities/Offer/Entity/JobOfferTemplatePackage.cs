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
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using CommonLibrary.Common.DocumentDB.Contracts;

    [DataContract]
    public class JobOfferTemplatePackage : DocDbEntity
    {
        [DataMember(Name = "JobOfferTemplatePackageID")]
        public string JobOfferTemplatePackageID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Description")]
        public string Description { get; set; }
        
        [DataMember(Name = "Status")]
        public JobOfferTemplatePackageStatus Status { get; set; }

        [DataMember(Name = "StatusReason")]
        public JobOfferTemplatePackageStatusReason StatusReason { get; set; }

        [DataMember(Name = "ValidFrom")]
        public DateTime? ValidFrom { get; set; }

        [DataMember(Name = "ValidTo")]
        public DateTime? ValidTo { get; set; }
        
        [DataMember(Name= "OriginalTemplatePackageID")]
        public string OriginalTemplatePackageID { get; set; }
        
        [DataMember(Name = "PreviousTemplatePackageID")]
        public string PreviousTemplatePackageID { get; set; }

        [DataMember(Name = "NextTemplatePackageID")]
        public string NextTemplatePackageID { get; set; }

        [DataMember(Name = "OptionalTokens")]
        public IList<string> OptionalTokens { get; set; }

        [DataMember(Name = "JobOfferTemplatePackageArtifacts")]
        public IList<JobOfferTemplatePackageArtifact> JobOfferTemplatePackageArtifacts { get; set; }
    }
}
