//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

/// <summary>
/// Namespace Offer Management Entities and Enums
/// </summary>
namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;
    using System.Collections.Generic;
    using System;

    [DataContract]
    public class JobOfferPackageDocument
    {
        [DataMember(Name = "JobOfferPackageDocumentID")]
        public string JobOfferPackageDocumentID { get; set; }

        [DataMember(Name = "JobOfferTemplateID")]
        public string JobOfferTemplateID { get; set; }

        [DataMember(Name = "Name")]
        public string Name { get; set; }

        [DataMember(Name = "Ordinal")]
        public int Ordinal { get; set; }

        [DataMember(Name = "IsOfferTextEditable")]
        public bool IsOfferTextEditable { get; set; }

        [DataMember(Name = "IsRequired")]
        public bool? IsRequired { get; set; }

        [DataMember(Name = "Tokens")]
        public IList<string> Tokens { get; set; }

        [DataMember(Name = "Artifacts")]
        public IList<JobOfferArtifact> Artifacts { get; set; }

        [DataMember(Name = "IsDeleted")]
        public bool? IsDeleted { get; set; }

        [DataMember(Name = "IsCandidateSignRequired")]
        public bool? IsCandidateSignRequired { get; set; }

        [DataMember(Name = "CandidateSigned")]
        public bool? CandidateSigned { get; set; }

        [DataMember(Name = "CandidateSignDate")]
        public DateTime? CandidateSignDate { get; set; }
    }
}
