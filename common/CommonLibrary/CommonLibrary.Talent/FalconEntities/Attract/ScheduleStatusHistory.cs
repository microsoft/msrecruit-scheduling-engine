//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Talent.FalconEntities.Attract
{
    using CommonLibrary.Common.DocumentDB.Contracts;
    using CommonLibrary.Talent.EnumSetModel;
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Text;

    [DataContract]
    public class ScheduleStatusHistory : DocDbEntity
    {
        [DataMember(Name = "ScheduleID", EmitDefaultValue = false, IsRequired = true)]
        public string ScheduleID { get; set; }

        [DataMember(Name = "PreviousScheduleStatus", EmitDefaultValue = false, IsRequired = false)]
        public ScheduleStatus PreviousScheduleStatus { get; set; }
        
        [DataMember(Name = "CurrentScheduleStatus", EmitDefaultValue = false, IsRequired = true)]
        public ScheduleStatus CurrentScheduleStatus { get; set; }

        [DataMember(Name = "DateTime")]
        public string DateTime { get; set; }
    }
}
