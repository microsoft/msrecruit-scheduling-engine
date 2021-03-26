//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "HR.TA..TalentEngagement")]
    public enum SkypeMeetingType
    {
        [EnumMember(Value = "none")]
        None = 0,
        [EnumMember(Value = "withoutCodeEditor")]
        WithoutCodeEditor = 1,
        [EnumMember(Value = "withCodeEditor")]
        WithCodeEditor = 2,
    }
}
