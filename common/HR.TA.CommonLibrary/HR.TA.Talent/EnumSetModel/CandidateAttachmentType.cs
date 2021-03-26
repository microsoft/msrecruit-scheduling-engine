//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum CandidateAttachmentType
    {
        [EnumMember(Value = "notSpecified")]
        NotSpecified = 0,
        [EnumMember(Value = "resume")]
        Resume = 1,
        [EnumMember(Value = "coverLetter")]
        CoverLetter = 2,
        [EnumMember(Value = "portfolio")]
        Portfolio = 3,
        [EnumMember(Value = "video")]
        Video = 4,
    }
}
