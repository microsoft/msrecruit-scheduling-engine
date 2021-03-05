//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum ArtifactType
    {
        [EnumMember(Value = "PDF")]
        PDF = 0,
        [EnumMember(Value = "DOC")]
        DOC = 1,
        [EnumMember(Value = "JPG")]
        JPG = 2,
        [EnumMember(Value = "DOCX")]
        DOCX = 3,
        [EnumMember(Value = "AVI")]
        AVI = 4,
        [EnumMember(Value = "MP4")]
        MP4 = 5,
    }
}
