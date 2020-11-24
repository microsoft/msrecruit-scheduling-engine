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
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Optionset;

    [ODataEntity(PluralName = "msdyn_jobofferesignatures", SingularName = "msdyn_jobofferesignature")]
    public class JobOfferESignature : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobofferesignatureid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "_msdyn_jobofferid_value")]
        public Guid? JobOfferId { get; set; }

        [DataMember(Name = "msdyn_Jobofferid")]
        public JobOffer JobOffer { get; set; }

        [DataMember(Name = "msdyn_externaldocumentreference")]
        public string ExternalDocumentReference { get; set; }

        [DataMember(Name = "msdyn_esignaturetype")]
        public ESignatureType? EsignatureType { get; set; }

        [DataMember(Name = "msdyn_signingcandidatename")]
        public string SigningCandidateName { get; set; }

        [DataMember(Name = "_msdyn_recruiteruserid_value")]
        public Guid? RecruiterUserId { get; set; }

        [DataMember(Name = "msdyn_recruiteruserid")]
        public Worker RecruiterUser { get; set; }

        [DataMember(Name = "msdyn_artifacttype")]
        public string ArtifactType { get; set; }
    }
}
