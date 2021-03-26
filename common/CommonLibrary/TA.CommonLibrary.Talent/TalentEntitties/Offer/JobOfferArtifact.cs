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
    using TA.CommonLibrary.Common.Provisioning.Entities.XrmEntities.Optionset;
    using TA.CommonLibrary.Common.XrmHttp.Model;

    [ODataEntity(PluralName = "msdyn_jobofferartifacts", SingularName = "msdyn_jobofferartifact")]
    public class JobOfferArtifact : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferartifactid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string Name { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_artifacttype")]
        public JobOfferArtifactType? ArtifactType { get; set; }

        [DataMember(Name = "msdyn_documenttype")]
        public JobOfferArtifactDocumentType? DocumentType { get; set; }

        [DataMember(Name = "msdyn_internalresourceuri")]
        public string InternalResourceUri { get; set; }

        [DataMember(Name = "msdyn_filelabel")]
        public string FileLabel { get; set; }

        [DataMember(Name = "_msdyn_uploadedby_value")]
        public Guid? UploadedByRecId { get; set; }

        [DataMember(Name = "msdyn_uploadedby")]
        public JobOfferArtifactUploadedBy? UploadedBy { get; set; }

        [DataMember(Name = "_msdyn_jobofferid_value")]
        public Guid? JobOfferRecId { get; set; }

        [DataMember(Name = "msdyn_jobofferid")]
        public JobOffer JobOffer { get; set; }

        [DataMember(Name = "_msdyn_jobofferpackagedocumentid_value")]
        public Guid? JobOfferPackageDocumentRecId { get; set; }

        [DataMember(Name = "msdyn_jobofferpackagedocumentId")]
        public JobOfferPackageDocument JobOfferPackageDocument { get; set; }

        [DataMember(Name = "msdyn_jobofferartifact_Annotations")]
        public IList<Annotation> Annotation { get; set; }
    }
}
