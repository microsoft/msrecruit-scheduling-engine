//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace HR.TA.Common.Provisioning.Entities.XrmEntities.Offer
{
    using System;
    using System.Runtime.Serialization;
    using System.ComponentModel.DataAnnotations;
    using HR.TA.Common.XrmHttp;
    using HR.TA.Common.Provisioning.Entities.XrmEntities.Common;
    using HR.TA.Common.Provisioning.Entities.XrmEntities.Optionset;

    [ODataEntity(PluralName = "msdyn_joboffertemplateparticipants", SingularName = "msdyn_joboffertemplateparticipant")]
    public class JobOfferTemplateParticipant : XrmODataEntity
    {
        [Key]
        [DataMember(Name = "msdyn_joboffertemplateparticipantid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "_msdyn_joboffertemplateid_value")]
        public Guid? JobOfferTemplateRecId { get; set; }

        [DataMember(Name = "msdyn_joboffertemplateid")]
        public JobOfferTemplate JobOfferTemplate { get; set; }

        [DataMember(Name = "msdyn_role")]
        public JobOfferTemplateParticipantRole? Role { get; set; }

        [DataMember(Name = "createdon")]
        public DateTime? CreatedAt { get; set; }

        [DataMember(Name = "modifiedon")]
        public DateTime? UpdatedAt { get; set; }

        [DataMember(Name = "_msdyn_workerid_value")]
        public Guid? WorkerRecId { get; set; }

        [DataMember(Name = "msdyn_workerId")]
        public Worker Worker { get; set; }
    }
}
