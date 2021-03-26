//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA..Common.Provisioning.Entities.XrmEntities.Offer
{
    using System.Runtime.Serialization;
    using HR.TA..Common.XrmHttp;
    using HR.TA..Common.XrmHttp.Model;

    /// <summary>
    /// Job Offer Package Artifact Annotation
    /// </summary>
    /// <seealso cref="HR.TA..Common.XrmHttp.XrmODataEntity" />
    [ODataEntity(PluralName = "annotations", SingularName = "annotation")]
    public class JobOfferTemplatePackageArtifactAnnotation : Annotation
    {
        /// <summary>
        /// Gets or sets the associate template package of the artifact.
        /// </summary>
        /// <value>
        /// The associate template package of the artifact.
        /// </value>
        [DataMember(Name = "objectid_msdyn_joboffertemplatepackageartifact")]
        public JobOfferTemplatePackageArtifact JobOfferTemplatePackageArtifact { get; set; }
    }
}
