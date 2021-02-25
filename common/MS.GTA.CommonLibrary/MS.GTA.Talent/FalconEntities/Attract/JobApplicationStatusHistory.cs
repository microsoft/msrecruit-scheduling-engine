//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Talent.FalconEntities.Attract
{
    using MS.GTA.Common.DocumentDB.Contracts;
    using MS.GTA.Talent.EnumSetModel;
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
