//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AssessmentResult.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
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
