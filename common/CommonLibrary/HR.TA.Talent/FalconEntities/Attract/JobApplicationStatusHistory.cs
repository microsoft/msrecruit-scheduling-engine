//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Talent.FalconEntities.Attract
{
    using HR.TA..Common.DocumentDB.Contracts;
    using HR.TA..Talent.EnumSetModel;
    using System.Runtime.Serialization;

    [DataContract]
    public class JobApplicationStatusHistory: DocDbEntity
    {
        [DataMember(Name = "JobApplicationID", EmitDefaultValue = false, IsRequired = true)]
        public string JobApplicationID { get; set; }

        [DataMember(Name = "PreviousJobApplicationStatus", EmitDefaultValue = false, IsRequired = false)]
        public JobApplicationActionType PreviousJobApplicationStatus { get; set; }

        [DataMember(Name = "CurrentJobApplicationStatus", EmitDefaultValue = false, IsRequired = true)]
        public JobApplicationActionType CurrentJobApplicationStatus { get; set; }

        [DataMember(Name = "DateTime")]
        public string DateTime { get; set; }
    }
}
