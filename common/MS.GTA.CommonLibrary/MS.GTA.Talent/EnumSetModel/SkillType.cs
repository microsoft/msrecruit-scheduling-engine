//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="SkillType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Talent.EnumSetModel
{
     using System.Runtime.Serialization;

    [DataContract]
    public enum SkillType
    {
        [EnumMember(Value = "culture")]
        Culture = 0,
        [EnumMember(Value = "role")]
        Role = 1        
    }
}
