﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.TalentEntities.Enum;

    [ODataEntity(PluralName = "msdyn_jobpostings", SingularName = "msdyn_jobposting")]
    public class JobPosting : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobpostingid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_jobposturl")]
        public string JobPostURI { get; set; }

        [DataMember(Name = "msdyn_publicationstartdate")]
        public DateTime? PublicationStartDate { get; set; }

        [DataMember(Name = "msdyn_publicationclosedate")]
        public DateTime? PublicationCloseDate { get; set; }

        [DataMember(Name = "msdyn_postingstatus")]
        public JobPostStatus? Status { get; set; }

        [DataMember(Name = "msdyn_postingstatusreason")]
        public JobPostStatusReason? StatusReason { get; set; }

        [DataMember(Name = "msdyn_postingsupplier")]
        public string JobPostSupplierName { get; set; }

        [DataMember(Name = "_msdyn_jobopeningid_value")]
        public Guid? JobOpeningId { get; set; }

        [DataMember(Name = "msdyn_JobopeningId")]
        public JobOpening JobOpening { get; set; }
    }
}
