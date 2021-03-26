//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA.Common.Provisioning.Entities.XrmEntities.Attract
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using HR.TA.Common.XrmHttp;
    using HR.TA.TalentEntities.Enum;

    [ODataEntity(PluralName = "msdyn_jobapplicationcomments", SingularName = "msdyn_jobapplicationcomment")]
    public class JobApplicationComment : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_jobapplicationcommentid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "msdyn_name")]
        public string XrmName { get; set; }

        [DataMember(Name = "_msdyn_jobapplicationid_value")]
        public Guid? JobApplicationId { get; set; }

        [DataMember(Name = "msdyn_JobapplicationId")]
        public JobApplication JobApplication { get; set; }

        [DataMember(Name = "_msdyn_jobopeningparticipantid_value")]
        public Guid? JobOpeningParticipantId { get; set; }

        [DataMember(Name = "msdyn_JobopeningparticipantId")]
        public JobOpeningParticipant JobOpeningParticipant { get; set; }

        [DataMember(Name = "msdyn_comment")]
        public string Comment { get; set; }

        [DataMember(Name = "msdyn_externalreference")]
        public string ExternalReference { get; set; }

        [DataMember(Name = "msdyn_source")]
        public TalentSource? Source { get; set; }

        [DataMember(Name = "msdyn_issyncedwithlinkedin")]
        public bool? IsSyncedWithLinkedIn { get; set; }
    }
}
