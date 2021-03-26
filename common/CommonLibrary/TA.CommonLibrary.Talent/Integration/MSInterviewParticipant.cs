//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;

namespace TA.CommonLibrary.Common.Integration.Contracts
{
    public enum InvitationResponseStatus
    {
        None = 0,
        Accepted = 1,
        TentativelyAccepted = 2,
        Declined = 3,
        Pending = 4,
        Sending = 5,
        ResendRequired = 6,
    }

    public enum JobParticipantRole
    {
        HiringManager = 0,
        Recruiter = 1,
        Interviewer = 2,
        Contributor = 3,
        AA = 4,
        HiringManagerDelegate = 5
    }

    /// <summary>
    /// MSInterviewParticipant
    /// </summary>
    public class MSInterviewParticipant
    {
        public string OID { get; set; }

        public JobParticipantRole Role { get; set; }

        public InvitationResponseStatus ParticipantResponseStatus { get; set; }

        public string ParticipantResponseComments { get; set; }

        public bool IsAssessmentCompleted { get; set; }

        public DateTime ParticipantResponseDateTime { get; set; }
    }
}
