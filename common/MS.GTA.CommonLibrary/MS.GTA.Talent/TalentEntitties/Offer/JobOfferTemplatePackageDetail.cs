//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using Common.XrmHttp;

    [ODataEntity(PluralName = "msdyn_joboffertemplatepackagedetails", SingularName = "msdyn_joboffertemplatepackagedetail")]
    public class JobOfferTemplatePackageDetail : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_joboffertemplatepackagedetailid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_istemplaterequired")]
        public bool? IsTemplateRequired { get; set; }

        [DataMember(Name = "msdyn_ordinal")]
        public int? Ordinal { get; set; }

        [DataMember(Name = "_msdyn_joboffertemplateid_value")]
        public Guid? JobOfferTemplateRecId { get; set; }

        [DataMember(Name = "msdyn_joboffertemplateid")]
        public JobOfferTemplate JobOfferTemplate { get; set; }

        [DataMember(Name = "_msdyn_originaltemplateid_value")]
        public Guid? OriginalTemplateRecId { get; set; }

        [DataMember(Name = "msdyn_originaltemplateid")]
        public JobOfferTemplate OriginalTemplate { get; set; }

        [DataMember(Name = "_msdyn_originaltemplatepackageid_value")]
        public Guid? OriginalTemplatePackageRecId { get; set; }

        [DataMember(Name = "msdyn_originaltemplatepackageid")]
        public JobOfferTemplatePackage OriginalTemplatePackage { get; set; }

        [DataMember(Name = "_msdyn_joboffertemplatepackageid_value")]
        public Guid? JobOfferTemplatePackageRecId { get; set; }

        [DataMember(Name = "msdyn_joboffertemplatepackageid")]
        public JobOfferTemplatePackage JobOfferTemplatePackage { get; set; }
    }
}
