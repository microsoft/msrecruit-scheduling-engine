//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.XrmData.Query.ViewModelExtensions
{
    using System;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp.Model;

    public static class HiringTeamMemberExtensions
    {
        public static JobOpeningParticipant ToEntity(this HiringTeamMember member) => new JobOpeningParticipant
        {
            Role = member.Role,
            Worker = member.ToWorkerEntity(),
        };

        public static Worker ToWorkerEntity(this HiringTeamMember member) => new Worker
        {
            EmailPrimary = member.Email,
            FullName = member.FullName,
            GivenName = member.GivenName,
            MiddleName = member.MiddleName,
            Surname = member.Surname,
            OfficeGraphIdentifier = member.ObjectId,
            PhonePrimary = null,
            Profession = member.Profession,
            SystemUser = new SystemUser
            {
                FullName = member.FullName,
                AzureActiveDirectoryObjectId = string.IsNullOrEmpty(member.ObjectId) ? null : (Guid?)Guid.Parse(member.ObjectId),
                PrimaryEmail = member.Email,
            },
        };

        /// <summary>The hiring team member to AssessmentUser model.</summary>
        /// <param name="member">The hiring team member.</param>
        /// <param name="role">The role: 1 - owner, 2 - contributor, 3 - readonly.</param>
        /// <returns>The <see cref="AssessmentUser"/>.</returns>
        public static AssessmentUser ToAssessmentUser(this HiringTeamMember member, int role = 1) => member == null ? null : new AssessmentUser
        {
            Name = member.FullName,
            UserId = member.ObjectId,
            Role = role
        };
    }
}
