//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
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
