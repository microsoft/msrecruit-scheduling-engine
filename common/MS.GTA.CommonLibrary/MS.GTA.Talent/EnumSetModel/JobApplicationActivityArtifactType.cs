//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobApplicationActivityArtifactType
    {
        [EnumMember(Value = "file")]
        File = 0,
        [EnumMember(Value = "email")]
        Email = 1,
        [EnumMember(Value = "meeting")]
        Meeting = 2,
        [EnumMember(Value = "comment")]
        Comment = 3,
        [EnumMember(Value = "image")]
        Image = 4,
        [EnumMember(Value = "uRL")]
        URL = 5,
        [EnumMember(Value = "video")]
        Video = 6
    }
}
