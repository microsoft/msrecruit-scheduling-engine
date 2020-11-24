//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobApplicationCommentExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp.Util;
    using MS.GTA.TalentEntities.Enum;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static class JobApplicationCommentExtensions
    {
        public static ApplicationNote ToViewModel(this JobApplicationComment jobApplicationComment) => jobApplicationComment == null ? null : new ApplicationNote
        {
            // TODO: support Private visibility!
            Visibility = CandidateNoteVisibility.Public,
            Id = jobApplicationComment.RecId.ToString(),
            Text = jobApplicationComment.Comment,
            OwnerObjectId = jobApplicationComment.JobOpeningParticipant?.Worker?.OfficeGraphIdentifier,
            OwnerEmail = jobApplicationComment.JobOpeningParticipant?.Worker?.EmailPrimary,
            OwnerName = jobApplicationComment.JobOpeningParticipant?.Worker?.FullName,
            CreatedDate = jobApplicationComment.XrmCreatedOn,
            CustomFields = jobApplicationComment.GetCustomFields(),
        };
    }
}
