//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Talent.FalconEntities.Attract
{
    using Common.DocumentDB.Contracts;
    using Talent.EnumSetModel;
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
