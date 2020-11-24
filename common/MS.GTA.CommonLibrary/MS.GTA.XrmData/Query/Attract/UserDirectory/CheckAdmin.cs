//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CheckAdmin.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.Common.Web.Contracts;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.XrmHttp.Model;
    
    public partial class XrmQuery : IQuery
    {
        /// <summary>
        /// Check if currect user  has given application role.
        /// </summary>
        /// <param name="applicationRole">Application Role</param>
        /// <returns>Flag that determines if user has given role</returns>
        public async Task<bool> IsAppAdmin(TalentApplicationRole applicationRole)
        {
            var adminClient = await this.GetAdminClient();
            TalentApplicationRole checkRole = (TalentApplicationRole)((int)applicationRole);

            if (applicationRole == TalentApplicationRole.AttractAdmin)
            {
                var systemUser = await this.GetSystemUserWithRoles(adminClient, this.userPrincipal.ObjectId);
                if (systemUser == null)
                {
                    return false;
                }
                return MapSystemUserRoleAppAdmin(applicationRole, systemUser.Roles);
            }

            var currentWorker = await this.GetWorker(adminClient, this.userPrincipal.ObjectId);
            if (currentWorker != null)
            {
                return (await this.GetRole(adminClient, applicationRole, currentWorker.RecId)) != null;
            }

            return false;
        }

        /// <summary>
        /// Maps the system user role application admin.
        /// </summary>
        /// <param name="applicationRole">The application role.</param>
        /// <param name="systemUserRoles">The system user roles.</param>
        /// <returns>True or False</returns>
        public static bool MapSystemUserRoleAppAdmin(TalentApplicationRole applicationRole, IEnumerable<Role> systemUserRoles)
        {
            switch (applicationRole)
            {
                case TalentApplicationRole.AttractAdmin:
                    return systemUserRoles.Any(u => u.ParentRootRoleId.Value == XrmRoleIds.RecruitingAdminRoleId);
                default:
                    return false;
            }
        }

        #region Private Methods
        private async Task<SystemUser> GetSystemUserWithRoles(IXrmHttpClient client, string userObjectId)
        {
            if (string.IsNullOrEmpty(userObjectId))
            {
                return null;
            }

            var systemUsers = await client.GetAll<SystemUser>(s => s.AzureActiveDirectoryObjectId.ToString() == userObjectId).ExecuteAndGetAsync(this.logger, "HcmUdsXrmGetSysUserRoles1");

            this.Trace.TraceInformation($"Found {systemUsers?.Result?.Count} system users for OID {userObjectId}");

            var systemUser = systemUsers?.Result?.FirstOrDefault();
            if (systemUser == null)
            {
                return null;
            }

            var userWithRoles = await client
                .Get<SystemUser>(
                    systemUser.SystemUserId.Value,
                    select: u => new { u.SystemUserId, u.BusinessUnitId },
                    expand: u => u.Roles)
                .ExecuteAndGetAsync(this.logger, "HcmUdsXrmGetSysUserRoles1");

            this.Trace.TraceInformation($"System user {systemUser.SystemUserId} OID {userObjectId} has roles: {string.Join(", ", userWithRoles.Roles.Select(r => $"{r.Name} ({r.RecId}, {r.ParentRootRoleId})"))}");

            return userWithRoles;
        }

        /// <summary>
        /// Get Worker
        /// </summary>
        /// <param name="client">Xrm client</param>
        /// <param name="identifier">User identifier</param>
        /// <returns>Worker</returns>
        private async Task<Worker> GetWorker(IXrmHttpClient client, string identifier)
        {
            var workers = await client.GetAll<Worker>(w => w.OfficeGraphIdentifier == identifier).ExecuteAndGetAsync(this.logger, "HcmUdsXrmGetWorker");
            return workers?.Result?.FirstOrDefault(w => w.SystemUserId != null);
        }

        private async Task<ApplicationRoleAssignment> GetRole(IXrmHttpClient client, TalentApplicationRole applicationRole, Guid? workerId)
        {
            var checkRole = (TalentApplicationRole)((int)applicationRole);
            var roles = await client.GetAll<ApplicationRoleAssignment>(role => role.TalentApplicationRole == checkRole && role.WorkerId == workerId).ExecuteAndGetAsync(this.logger, "HcmUdsXrmGetRole");
            return roles?.Result?.FirstOrDefault();
        }
        #endregion
    }
}
