//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOpeningApprovalParticipantExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp.Util;
    using MS.GTA.XrmData.Query.Attract.Dashboard;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static class JobOpeningApprovalParticipantExtensions
    {
        /// <summary>The to view model.</summary>
        /// <param name="jobOpeningApprovalParticipant">The job approval participant.</param>
        /// <returns>The <see cref="JobApprovalParticipant"/>.</returns>
        public static JobApprovalParticipant ToViewModel(this JobOpeningApprovalParticipant jobOpeningApprovalParticipant) => jobOpeningApprovalParticipant == null || jobOpeningApprovalParticipant.Worker == null ? null : new JobApprovalParticipant
        {
            ObjectId = jobOpeningApprovalParticipant.Worker.OfficeGraphIdentifier,
            FullName = jobOpeningApprovalParticipant.Worker.FullName ?? jobOpeningApprovalParticipant.Worker.SystemUser?.FullName,
            GivenName = jobOpeningApprovalParticipant.Worker.GivenName ?? jobOpeningApprovalParticipant.Worker.SystemUser?.FirstName,
            MiddleName = jobOpeningApprovalParticipant.Worker.MiddleName ?? jobOpeningApprovalParticipant.Worker.SystemUser?.MiddleName,
            Surname = jobOpeningApprovalParticipant.Worker.Surname ?? jobOpeningApprovalParticipant.Worker.SystemUser?.LastName,
            Email = jobOpeningApprovalParticipant.Worker.EmailPrimary ?? jobOpeningApprovalParticipant.Worker.SystemUser?.PrimaryEmail,
            LinkedIn = jobOpeningApprovalParticipant.Worker.LinkedInIdentity,
            WorkPhone = jobOpeningApprovalParticipant.Worker.PhonePrimary,
            Profession = jobOpeningApprovalParticipant.Worker.Profession,

            // TODO: abthakur - Sync with Sam regarding this
            // [following the same pattern as in job opening participant entity extension]
            // ExternalWorkerId = worker?.ExternalReference, [following the same pattern as in job opening participant entity extension]
            // ExternalWorkerSource = worker?.Source,
            Comment = jobOpeningApprovalParticipant.Comment,
            JobApprovalStatus = jobOpeningApprovalParticipant.JobApprovalStatus,
            CustomFields = jobOpeningApprovalParticipant.GetCustomFields()
        };

        public static AssignedTask ToAssignedActivityViewModel(this JobOpeningApprovalParticipant jobOpeningApprovalParticipant) => jobOpeningApprovalParticipant == null ? null : new AssignedTask
        {
            AssignedTaskType = AssignedTaskType.JobApproval,
            Date = jobOpeningApprovalParticipant.XrmCreatedOn,
            RecId = jobOpeningApprovalParticipant.RecId,
        };
    }
}
