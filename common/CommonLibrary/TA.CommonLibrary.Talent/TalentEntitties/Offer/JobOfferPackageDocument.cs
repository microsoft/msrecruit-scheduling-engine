//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace TA.CommonLibrary.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using TA.CommonLibrary.Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_jobofferpackagedocuments", SingularName = "msdyn_jobofferpackagedocument")]
    public class JobOfferPackageDocument : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferpackagedocumentid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string Name { get; set; }

        [DataMember(Name = "msdyn_candidatesignaturedate")]
        public DateTime? CandidateSignatureDate { get; set; }

        [DataMember(Name = "msdyn_issignedbycandidate")]
        public bool? IsSignedByCandidate { get; set; }

        [DataMember(Name = "msdyn_iscandidatesignaturerequired")]
        public bool? IsCandidateSignatureRequired { get; set; }

        [DataMember(Name = "msdyn_isdeleted")]
        public bool? IsDeleted { get; set; }

        [DataMember(Name = "msdyn_isoffertexteditable")]
        public bool? IsOfferTextEditable { get; set; }

        [DataMember(Name = "msdyn_isrequired")]
        public bool? IsRequired { get; set; }

        [DataMember(Name = "msdyn_ordinal")]
        public int? Ordinal { get; set; }

        [DataMember(Name = "_msdyn_joboffertemplateid_value")]
        public Guid? JobOfferTemplateRecId { get; set; }

        [DataMember(Name = "msdyn_joboffertemplateid")]
        public JobOfferTemplate JobOfferTemplate { get; set; }

        [DataMember(Name = "_msdyn_jobofferid_value")]
        public Guid? JobOfferRecId { get; set; }

        [DataMember(Name = "msdyn_jobofferid")]
        public JobOffer JobOffer { get; set; }

        [DataMember(Name = "msdyn_packagedocument_artifact_artifactid")]
        public IList<JobOfferArtifact> Artifacts { get; set; }

        [DataMember(Name = "msdyn_jobofferpackagedocument_joboffertoken")]
        public IList<JobOfferToken> JobOfferTokens { get; set; }
    }
}
