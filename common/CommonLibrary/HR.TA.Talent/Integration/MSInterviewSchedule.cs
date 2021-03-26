//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace HR.TA..Common.Integration.Contracts
{
    public enum CandidateExternalSource
    {
        Internal = 0,
        MSDataMall = 1,
        LinkedIn = 2,
        Greenhouse = 3,
        ICIMS = 4
    }

    public enum ScheduleStatus
    {
        NotScheduled = 0,
        Saved = 1,
        Queued = 2,
        Sent = 3,
        FailedToSend = 4,
        Delete = 5,
    }

    public enum InterviewMode
    {
        None,
        FaceToFace,
        OnlineCall,
    }

    /// <summary>
    /// MSInterviewSchedule
    /// </summary>
    public class MSInterviewSchedule
    {
        public string ScheduleID { get; set; }

        public string JobApplicationID { get; set; }

        public string ExternalCandidateID { get; set; }

        public CandidateExternalSource? ExternalCandidateSource { get; set; }

        public List<MSInterviewParticipant> InterviewParticipants { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public ScheduleStatus ScheduleStatus { get; set; }

        public InterviewMode ModeOfInterview { get; set; }

        public string ScheduleRequesterOID { get; set; }
    }
}
