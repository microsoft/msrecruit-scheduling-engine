//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "TalentEngagement")]
    public enum FinalScheduleStatus
    {
        [EnumMember(Value = "None")]
        None = 0,
        [EnumMember(Value = "Scheduled")]
        Scheduled = 1,
        [EnumMember(Value = "Draft")]
        Draft = 2,
        [EnumMember(Value = "Sending")]
        Sending = 3,
        [EnumMember(Value = "Failed")]
        Failed = 4,
    }
}
