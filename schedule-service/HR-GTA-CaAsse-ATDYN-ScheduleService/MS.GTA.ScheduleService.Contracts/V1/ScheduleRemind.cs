//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Runtime.Serialization;
using MS.GTA.Common.DocumentDB.Contracts;
using MS.GTA.Talent.EnumSetModel;

namespace MS.GTA.Talent.FalconEntities.Attract
{
    /// <summary>
    /// Schedule Remind Entity
    /// </summary>
    [DataContract]
    public class ScheduleRemind : DocDbEntity
    {
        /// <summary>
        /// Gets or sets the schedule ID.
        /// </summary>
        [DataMember(Name = "ScheduleID", EmitDefaultValue = false, IsRequired = true)]
        public string ScheduleID { get; set; }

        /// <summary>
        /// Gets or sets the email sent Date and time.
        /// </summary>
        [DataMember(Name = "DateTime")]
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the email is sent or not.
        /// </summary>
        [DataMember(Name = "IsReminderSent", EmitDefaultValue = false, IsRequired = false)]
        public bool IsReminderSent { get; set; }
    }
}
