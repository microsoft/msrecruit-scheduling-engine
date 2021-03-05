//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Talent.EnumSetModel
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
