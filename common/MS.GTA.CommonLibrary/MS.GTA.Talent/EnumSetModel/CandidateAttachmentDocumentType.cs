//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum CandidateAttachmentDocumentType
    {
        [EnumMember(Value = "pDF")]
        PDF = 0,
        [EnumMember(Value = "dOC")]
        DOC = 1,
        [EnumMember(Value = "jPG")]
        JPG = 2,
        [EnumMember(Value = "dOCX")]
        DOCX = 3,
        [EnumMember(Value = "AVI")]
        AVI = 4,
        [EnumMember(Value = "MP4")]
        MP4 = 5,
        [EnumMember(Value = "HTML")]
        HTML = 6,
        [EnumMember(Value = "TXT")]
        TXT = 7,
        [EnumMember(Value = "ODT")]
        ODT = 8,
        [EnumMember(Value = "RTF")]
        RTF = 9,
        [EnumMember(Value = "PPTX")]
        PPTX = 10,
    }
}
