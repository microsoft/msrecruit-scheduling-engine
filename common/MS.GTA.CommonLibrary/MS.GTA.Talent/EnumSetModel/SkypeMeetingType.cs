//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="SkypeMeetingType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
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
