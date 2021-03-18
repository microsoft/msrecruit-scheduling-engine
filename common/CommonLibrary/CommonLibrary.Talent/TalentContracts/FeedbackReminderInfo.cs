//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Collections.Generic;

    public class FeedbackReminderInfo
    {
        public string ApplicationId { get; set; }

        public string JobId { get; set; }

        public string JobTitle { get; set; }

        public string CandidateName { get; set; }

        public int? StageOrder { get; set; }

        public HiringTeamMember HiringManager { get; set; }

        public List<HiringTeamMember> ActivityParticipants { get; set; }

    }
}
