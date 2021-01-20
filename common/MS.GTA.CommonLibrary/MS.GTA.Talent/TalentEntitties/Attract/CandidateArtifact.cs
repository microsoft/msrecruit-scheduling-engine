//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.XrmHttp.Model;
    using MS.GTA.TalentEntities.Enum;

    [ODataEntity(PluralName = "msdyn_candidateartifacts", SingularName = "msdyn_candidateartifact")]
    public class CandidateArtifact : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_candidateartifactid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_autonumber")]
        public string Autonumber { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string XrmName { get; set; }

        [DataMember(Name = "_msdyn_candidateid_value")]
        public Guid? CandidateId { get; set; }

        [DataMember(Name = "msdyn_CandidateId")]
        public Candidate Candidate { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationid_value")]
        public Guid? JobApplicationId { get; set; }

        [DataMember(Name = "msdyn_JobapplicationId")]
        public JobApplication JobApplication { get; set; }

        [DataMember(Name = "msdyn_source")]
        public TalentSource? Source { get; set; }

        [DataMember(Name = "msdyn_externalreference")]
        public string ExternalReference { get; set; }

        [DataMember(Name = "msdyn_artifactlink")]
        public string ArtifactLink { get; set; }

        [DataMember(Name = "msdyn_artifactname")]
        public string ArtifactName { get; set; }

        [DataMember(Name = "msdyn_blobreference")]
        public string BlobReference { get; set; }

        [DataMember(Name = "msdyn_contenttype")]
        public string ContentType { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_uploaderpuid")]
        public string UploaderPuid { get; set; }

        [DataMember(Name = "msdyn_artifactpurpose")]
        public CandidateAttachmentType? ArtifactPurpose { get; set; }

        [DataMember(Name = "msdyn_size")]
        public int? Size { get; set; }

        [DataMember(Name = "msdyn_candidateartifact_Annotations")]
        public IList<Annotation> Annotation { get; set; }
    }
}
