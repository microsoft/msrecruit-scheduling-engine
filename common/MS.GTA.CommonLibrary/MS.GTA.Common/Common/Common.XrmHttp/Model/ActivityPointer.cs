//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using MS.GTA.Common.XrmHttp;

    [ODataEntity(PluralName = "activitypointers", SingularName = "activitypointer")]
    public class ActivityPointer : XrmODataEntity
    { 
        [Key]
        [DataMember(Name = "activityid")]
        public Guid? RecId { get; set; }

        [DataMember(Name = "scheduledstart")]
        public DateTime? ScheduledStart { get; set; }

        [DataMember(Name = "scheduledend")]
        public DateTime? ScheduledEnd { get; set; }

        [DataMember(Name = "scheduleddurationminutes")]
        public int? ScheduledDurationMinutes { get; set; }

        [DataMember(Name = "subject")]
        public string Subject { get; set; }

        [DataMember(Name = "statecode")]
        public int? StateCode { get; set; }

        [DataMember(Name = "deliveryprioritycode")]
        public int? DeliveryPriorityCode { get; set; }

        [DataMember(Name = "activitytypecode")]
        public string ActivityTypeCode { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "activitypointer_activity_parties")]
        public IList<ActivityParty> ActivityParties { get; set; }

        [DataMember(Name = "activity_pointer_task")]
        public IList<TaskActivity> Tasks { get; set; }

        [DataMember(Name = "activity_pointer_email")]
        public IList<Email> Emails { get; set; }

       }
}
