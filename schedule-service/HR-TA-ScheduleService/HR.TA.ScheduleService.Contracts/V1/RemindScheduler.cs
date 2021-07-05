//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Talent.TalentContracts.InterviewService
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The Remind Scheduler data contract.
    /// </summary>
    [DataContract]
    public class RemindScheduler
    {
        /// <summary>Gets or sets the Job Opening Position.</summary>
        [DataMember(Name = "PositionTitle", EmitDefaultValue = false, IsRequired = false)]
        public string PositionTitle { get; set; }

        /// <summary>Gets or sets the External Job Opening ID.</summary>
        [DataMember(Name = "ExternalJobOpeningID", EmitDefaultValue = false, IsRequired = false)]
        public string ExternalJobOpeningID { get; set; }

        /// <summary> Gets or sets the Scheduler Oid/// </summary>
        [DataMember(Name = "SchedulerOID", EmitDefaultValue = false, IsRequired = false)]
        public string SchedulerOID { get; set; }

        /// <summary> Gets or sets the Scheduler Name/// </summary>
        [DataMember(Name = "SchedulerName", EmitDefaultValue = false, IsRequired = false)]
        public string SchedulerName { get; set; }

        /// <summary> Gets or sets the Scheduler Email/// </summary>
        [DataMember(Name = "SchedulerEmail", EmitDefaultValue = false, IsRequired = false)]
        public string SchedulerEmail { get; set; }
    }
}