//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum JobParticipantRole
    {
        [EnumMember(Value = "hiringManager")]
        HiringManager = 0,

        [EnumMember(Value = "recruiter")]
        Recruiter = 1,

        [EnumMember(Value = "interviewer")]
        Interviewer = 2,

        [EnumMember(Value = "contributor")]
        Contributor = 3,

        [EnumMember(Value = "AA")]
        AA = 4,
        [EnumMember(Value = "hiringManagerDelegate")]
        HiringManagerDelegate = 5
    }
}
