//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="WorkerExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.Web.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static class WorkerExtensions
    {
        public static HiringTeamMember ToViewModel(this Worker worker) => new HiringTeamMember
        {
            ObjectId = worker?.OfficeGraphIdentifier,
            FullName = worker?.FullName ?? worker?.SystemUser?.FullName,
            GivenName = worker?.GivenName ?? worker?.SystemUser?.FirstName,
            MiddleName = worker?.MiddleName ?? worker?.SystemUser?.MiddleName,
            Surname = worker?.Surname ?? worker?.SystemUser?.LastName,
            Email = worker?.EmailPrimary ?? worker?.SystemUser?.PrimaryEmail,
            ////LinkedIn = worker?.LinkedInIdentity,
            WorkPhone = worker?.PhonePrimary,
            Profession = worker?.Profession,
            ////ExternalWorkerId = worker?.ExternalReference,
            ////ExternalWorkerSource = worker?.Source,
        };

        public static MS.GTA.Common.TalentAttract.Contract.Delegate ToDelegateModel(this Worker worker, string onBehalfOfUserObjectId)
        {
            return new MS.GTA.Common.TalentAttract.Contract.Delegate
            {
                ObjectId = worker?.OfficeGraphIdentifier,
                FullName = worker?.FullName ?? worker?.SystemUser?.FullName,
                GivenName = worker?.GivenName ?? worker?.SystemUser?.FirstName,
                MiddleName = worker?.MiddleName ?? worker?.SystemUser?.MiddleName,
                Surname = worker?.Surname ?? worker?.SystemUser?.LastName,
                Email = worker?.EmailPrimary ?? worker?.SystemUser?.PrimaryEmail,
                ////LinkedIn = worker?.LinkedInIdentity,
                WorkPhone = worker?.PhonePrimary,
                Profession = worker?.Profession,
                ////ExternalWorkerId = worker?.ExternalReference,
                ////ExternalWorkerSource = worker?.Source,
                OnBehalfOfUserObjectId = onBehalfOfUserObjectId
            };
        }

        public static System.Guid? GetXrmRoleRecIdForApplicationRole(this TalentApplicationRole applicationRole)
        {
            switch (applicationRole)
            {
                case TalentApplicationRole.AttractAdmin:
                    return XrmRoleIds.RecruitingAdminRoleId;
                case TalentApplicationRole.AttractHiringManager:
                    return XrmRoleIds.HiringManagerRoleId;
                case TalentApplicationRole.AttractReader:
                    return XrmRoleIds.RecruitingReadAllRoleId;
                case TalentApplicationRole.AttractRecruiter:
                    return XrmRoleIds.RecruiterRoleId;
            }
            return null;
        }

        public static TalentApplicationRole? GetApplicationRoleForXrmRoleRecId(this System.Guid? roleRecId)
        {
            if (roleRecId == XrmRoleIds.RecruitingAdminRoleId)
            {
                return TalentApplicationRole.AttractAdmin;
            }
            if (roleRecId == XrmRoleIds.HiringManagerRoleId)
            {
                return TalentApplicationRole.AttractHiringManager;
            }
            if (roleRecId == XrmRoleIds.RecruitingReadAllRoleId)
            {
                return TalentApplicationRole.AttractReader;
            }
            if (roleRecId == XrmRoleIds.RecruiterRoleId)
            {
                return TalentApplicationRole.AttractRecruiter;
            }
            return null;
        }
    }
}
