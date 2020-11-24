//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Optionset;

    [ODataEntity(PluralName = "msdyn_joboffertemplatepackages", SingularName = "msdyn_joboffertemplatepackage")]
    public class JobOfferTemplatePackage : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_joboffertemplatepackageid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string Name { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_status")]
        public JobOfferTemplatePackageStatus? Status { get; set; }

        [DataMember(Name = "msdyn_statusreason")]
        public JobOfferTemplatePackageStatusReason? StatusReason { get; set; }

        [DataMember(Name = "msdyn_validfrom")]
        public DateTime? ValidFrom { get; set; }

        [DataMember(Name = "msdyn_validto")]
        public DateTime? ValidTo { get; set; }

        [DataMember(Name = "_msdyn_originaltemplatepackageid_value")]
        public Guid? OriginalTemplatePackageRecId { get; set; }

        [DataMember(Name = "msdyn_originaltemplatepackageid")]
        public JobOfferTemplatePackage OriginalTemplatePackage { get; set; }

        [DataMember(Name = "_msdyn_previoustemplatepackageid_value")]
        public Guid? PreviousTemplatePackageRecId { get; set; }

        [DataMember(Name = "msdyn_previoustemplatepackageid")]
        public JobOfferTemplatePackage PreviousTemplatePackage { get; set; }

        [DataMember(Name = "_msdyn_nexttemplatepackageid_value")]
        public Guid? NextTemplatePackageRecId { get; set; }

        [DataMember(Name = "msdyn_nexttemplatepackageid")]
        public JobOfferTemplatePackage NextTemplatePackage { get; set; }

        [DataMember(Name = "msdyn_joboffertemplatepackage_joboffer_joboffertemplatepackageid")]
        public IList<JobOffer> JobOffers { get; set; }

        [DataMember(Name = "msdyn_joboffertemplatepackage_joboffertemplatep")]
        public IList<JobOfferTemplatePackageDetail> JobOfferTemplatePackageDetails { get; set; }

        [DataMember(Name = "msdyn_joboffertemplatepackage_optionaltoken")]
        public IList<JobOfferToken> OptionalTokens { get; set; }

        /// <summary>
        /// Gets or sets the associated template package artifacts of the template package.
        /// </summary>
        /// <value>
        /// The associated template package artifacts of the template package.
        /// </value>
        [DataMember(Name = "msdyn_templatepackage_packageartifact_artifact")]
        public IList<JobOfferTemplatePackageArtifact> JobOfferTemplatePackageArtifacts { get; set; }
    }
}
