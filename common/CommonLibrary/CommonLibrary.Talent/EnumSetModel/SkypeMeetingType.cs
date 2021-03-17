//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
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
