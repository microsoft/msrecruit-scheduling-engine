//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Collections.Generic;

    public class FeedbackNotificationInfo
    {
        public string ApplicationId { get; set; }

        public string JobId { get; set; }

        public string JobTitle { get; set; }

        public string CandidateName { get; set; }

        public List<HiringTeamMember> HiringTeam { get; set; }
    }
}
