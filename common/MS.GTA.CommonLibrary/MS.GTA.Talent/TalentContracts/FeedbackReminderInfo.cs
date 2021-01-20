﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="FeedbackReminder.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
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
