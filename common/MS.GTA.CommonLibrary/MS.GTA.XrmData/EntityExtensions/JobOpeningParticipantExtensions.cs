//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOpeningParticipantExtensions.cs">
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
    using System.Linq;
    using System.Text;


    public static class JobOpeningParticipantExtensions
    {
        public static HiringTeamMember ToViewModel(this JobOpeningParticipant participant)
        {
            var hiringTeamMember = participant?.Worker?.ToViewModel();
            if (hiringTeamMember != null)
            {
                hiringTeamMember.Role = participant.Role.GetValueOrDefault();
            }
            return hiringTeamMember;
        }

        public static TeamMember ToTeamMemberViewModel(this JobOpeningParticipant participant)
        {
            if (participant != null && participant.Worker != null)
            {
                var worker = participant.Worker;
                var teamMember = new TeamMember()
                {
                    Id = worker.OfficeGraphIdentifier,
                    Role = participant.Role.GetValueOrDefault(),
                    GivenName = worker.FullName,
                    Name = worker.FullName,
                    Email = worker.EmailPrimary,
                    Title = worker.Profession,
                    CustomFields = participant.GetCustomFields()
                };

                return teamMember;
            }

            return null;
        }

        public static JobOpeningTemplateParticipant ToTemplateViewModel(this JobOpeningParticipant participant) => participant == null ? null : new JobOpeningTemplateParticipant
        {
            Id = participant.RecId.ToString(),
            UserObjectId = participant.Worker?.OfficeGraphIdentifier,
            GroupObjectId = participant.GroupObjectId,
            TenantObjectId = participant.TenantObjectId,
            IsDefault = participant.IsDefaultTemplate.GetValueOrDefault(),
            CanEdit = participant.CanEditTemplate.GetValueOrDefault(),
            CustomFields = participant.GetCustomFields()
        };

        /// <summary>Check if any hiring manger or recruiter has been updated.</summary>
        /// <param name="existingParticipants">The existing job opening participant.</param>
        /// <param name="updatedParticipants">The updated job opening participant.</param>
        /// <returns>The <see cref="HiringTeamMember"/>.</returns>
        public static bool IsAnyHiringMangerOrRecruiterUpdated(this IEnumerable<JobOpeningParticipant> existingParticipants, IEnumerable<JobOpeningParticipant> updatedParticipants)
        {
            var existingHiringManagersAndRecruiters = existingParticipants?.Where(jop => jop.Role == JobParticipantRole.HiringManager || jop.Role == JobParticipantRole.Recruiter);
            var updatedHiringManagersAndRecruiters = updatedParticipants?.Where(jop => jop.Role == JobParticipantRole.HiringManager || jop.Role == JobParticipantRole.Recruiter);

            return existingHiringManagersAndRecruiters?.All(ejop => updatedHiringManagersAndRecruiters?.Any(ujop => ujop.Worker.RecId == ejop.WorkerId) == true) != true
                    || updatedHiringManagersAndRecruiters?.All(ujop => existingHiringManagersAndRecruiters?.Any(ejop => ujop.Worker.RecId == ejop.WorkerId) == true) != true;
        }
    }
}
