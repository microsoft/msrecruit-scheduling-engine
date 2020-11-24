//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
{
    using MS.GTA.Talent.FalconEntities.Attract;
    using System.Runtime.Serialization;

    /// <summary>
    /// Enumeration of the actions that can be performed on a <see cref="JobApplicationSchedule"/>
    /// </summary>
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobApplicationScheduleAction
    {
        /// <summary>
        /// Schedule is created.
        /// </summary>
        [EnumMember(Value = "create")]
        Create = 0,
        /// <summary>
        /// Schedule is updated.
        /// </summary>
        [EnumMember(Value = "update")]
        Update = 1,
        /// <summary>
        /// Schedule is deleted.
        /// </summary>
        [EnumMember(Value = "delete")]
        Delete = 2,
        /// <summary>
        /// Schedule is sent to the interviewer(s).
        /// </summary>
        [EnumMember(Value = "sendtointerviewers")]
        SendToInterviewers = 3,
        /// <summary>
        /// Schedule is sent to candidate.
        /// </summary>
        [EnumMember(Value = "sendtocandidate")]
        SendToCandidate = 4
    }
}
