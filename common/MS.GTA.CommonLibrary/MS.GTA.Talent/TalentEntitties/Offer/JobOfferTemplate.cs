//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
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
    using MS.GTA.Common.XrmHttp.Model;

    [ODataEntity(PluralName = "msdyn_joboffertemplates", SingularName = "msdyn_joboffertemplate")]
    public class JobOfferTemplate : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_joboffertemplateid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_autonumber")]
        public string AutoNumber { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string Name { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_templateuri")]
        public string TemplateUri { get; set; }

        [DataMember(Name = "msdyn_content")]
        public string Content { get; set; }

        [DataMember(Name = "msdyn_accesslevel")]
        public JobOfferTemplateAccessLevel? AccessLevel { get; set; }

        [DataMember(Name = "msdyn_joboffertemplatestatus")]
        public JobOfferTemplateStatus? Status { get; set; }

        [DataMember(Name = "msdyn_joboffertemplatestatusreason")]
        public JobOfferTemplateStatusReason? StatusReason { get; set; }

        [DataMember(Name = "msdyn_validfrom")]
        public DateTime? ValidFrom { get; set; }

        [DataMember(Name = "msdyn_validto")]
        public DateTime? ValidTo { get; set; }

        [DataMember(Name = "msdyn_activateddate")]
        public DateTime? ActivatedDate { get; set; }

        [DataMember(Name = "msdyn_isoffertexteditable")]
        public bool? IsOfferTextEditable { get; set; }

        [DataMember(Name = "_msdyn_originaltemplateid_value")]
        public Guid? OriginalTemplateRecId { get; set; }

        [DataMember(Name = "msdyn_originaltemplateid")]
        public JobOfferTemplate OriginalTemplate { get; set; }

        [DataMember(Name = "_msdyn_previoustemplateid_value")]
        public Guid? PreviousTemplateRecId { get; set; }

        [DataMember(Name = "msdyn_previoustemplateid")]
        public JobOfferTemplate PreviousTemplate { get; set; }

        [DataMember(Name = "_msdyn_nexttemplateid_value")]
        public Guid? NextTemplateRecId { get; set; }

        [DataMember(Name = "msdyn_nexttemplateid")]
        public JobOfferTemplate NextTemplate { get; set; }

        [DataMember(Name = "msdyn_joboffertemplate_joboffertemplateparticip")]
        public IList<JobOfferTemplateParticipant> Participants { get; set; }

        [DataMember(Name = "msdyn_joboffertemplate_joboffertemplatesection")]
        public IList<JobOfferTemplateSection> Sections { get; set; }

        [DataMember(Name = "msdyn_joboffertemplate_joboffertoken")]
        public IList<JobOfferToken> JobOfferTokens { get; set; }

        [DataMember(Name = "msdyn_joboffertemplate_joboffer_joboffertemplateid")]
        public IList<JobOffer> JobOffers { get; set; }

        [DataMember(Name = "msdyn_joboffertemplate_jobofferpackagedocument_joboffertemplateid")]
        public IList<JobOfferPackageDocument> JobOfferPackageDocuments { get; set; }

        [DataMember(Name = "msdyn_joboffertemplate_Annotations")]
        public IList<Annotation> Annotation { get; set; }
    }
}
