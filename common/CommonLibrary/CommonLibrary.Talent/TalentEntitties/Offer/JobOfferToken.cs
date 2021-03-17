//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace CommonLibrary.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;
    using CommonLibrary.Common.XrmHttp;
    using CommonLibrary.Common.Provisioning.Entities.XrmEntities.Optionset;

    [ODataEntity(PluralName = "msdyn_joboffertokens", SingularName = "msdyn_joboffertoken")]
    public class JobOfferToken : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_joboffertokenid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string Name { get; set; }

        [DataMember(Name = "msdyn_description")]
        public string Description { get; set; }

        [DataMember(Name = "msdyn_tokentype")]
        public JobOfferTokenType? TokenType { get; set; }

        [DataMember(Name = "msdyn_datatype")]
        public JobOfferTokenDataType? DataType { get; set; }

        [DataMember(Name = "msdyn_sectionname")]
        public string SectionName { get; set; }

        [DataMember(Name = "msdyn_sectionid")]
        public string SectionId { get; set; }

        [DataMember(Name = "msdyn_ordinalnumber")]
        public int? OrdinalNumber { get; set; }

        [DataMember(Name = "msdyn_iseditable")]
        public bool? IsEditable { get; set; }

        [DataMember(Name = "msdyn_introducedversion")]
        public int? IntroducedVersion { get; set; }

        [DataMember(Name = "_msdyn_joboffertemplate_joboffertoken_originatingtemplateid_value")]
        public Guid? OriginalTemplateRecId { get; set; }

        [DataMember(Name = "msdyn_joboffertemplate_joboffertoken_originatingtemplateid")]
        public JobOfferTemplate OriginalTemplate { get; set; }

        [DataMember(Name = "_msdyn_jobofferpackagedocument_joboffertoken_value")]
        public Guid? JobOfferPackageDocumentRecId { get; set; }

        [DataMember(Name = "msdyn_jobofferpackagedocument_joboffertoken")]
        public JobOfferPackageDocument JobOfferPackageDocument { get; set; }

        [DataMember(Name = "msdyn_joboffertemplate_joboffertoken")]
        public IList<JobOfferTemplate> JobOfferTemplates { get; set; }

        [DataMember(Name = "msdyn_joboffertemplatetoken_joboffer")]
        public IList<JobOfferTemplateToken> JobOfferTemplateToken { get; set; }

        [DataMember(Name = "msdyn_joboffertokenvalue_joboffertoken")]
        public IList<JobOfferTokenValue> JobOfferTokenValues { get; set; }
    }
}
