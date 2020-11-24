//----------------------------------------------------------------------------
// <copyright file="JobOfferTemplatePackageArtifact.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Optionset;
    using MS.GTA.Common.XrmHttp.Model;

    /// <summary>
    /// Job Offer Package Artifact
    /// </summary>
    /// <seealso cref="MS.GTA.Common.XrmHttp.XrmODataEntity" />
    [ODataEntity(PluralName = "msdyn_joboffertemplatepackageartifacts", SingularName = "msdyn_joboffertemplatepackageartifact")]
    public class JobOfferTemplatePackageArtifact : XrmODataEntity
    {
        /// <summary>
        /// Gets or sets the record identifier.
        /// </summary>
        /// <value>
        /// The record identifier.
        /// </value>
        [Key]
        [DataMember(Name = "msdyn_joboffertemplatepackageartifactid")]
        public Guid? RecId { get; set; }

        /// <summary>
        /// Gets or sets the name of the artifact.
        /// </summary>
        /// <value>
        /// The name of the artifact.
        /// </value>
        [DataMember(Name = "msdyn_name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of the artifact.
        /// </summary>
        /// <value>
        /// The description of the artifact.
        /// </value>
        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the document type of the artifact.
        /// </summary>
        /// <value>
        /// The document type of the artifact.
        /// </value>
        [DataMember(Name = "msdyn_documenttype")]
        public JobOfferArtifactDocumentType? DocumentType { get; set; }

        /// <summary>
        /// Gets or sets the file label of the artifact.
        /// </summary>
        /// <value>
        /// The file label of the artifact.
        /// </value>
        [DataMember(Name = "msdyn_filelabel")]
        public string FileLabel { get; set; }

        /// <summary>
        /// Gets or sets the associate template package identifier of the artifact.
        /// </summary>
        /// <value>
        /// The associate template package identifier of the artifact.
        /// </value>
        [DataMember(Name = "_msdyn_joboffertemplatepackageid_value")]
        public Guid? JobOfferTemplatePackageRecId { get; set; }

        /// <summary>
        /// Gets or sets the associate template package of the artifact.
        /// </summary>
        /// <value>
        /// The associate template package of the artifact.
        /// </value>
        [DataMember(Name = "msdyn_joboffertemplatepackageid")]
        public JobOfferTemplatePackage JobOfferTemplatePackage { get; set; }

        /// <summary>
        /// Gets or sets the annotation of the artifact.
        /// </summary>
        /// <value>
        /// The annotation of the artifact.
        /// </value>
        [DataMember(Name = "msdyn_joboffertemplatepackageartifact_Annotations")]
        public IList<Annotation> Annotation { get; set; }
    }
}
