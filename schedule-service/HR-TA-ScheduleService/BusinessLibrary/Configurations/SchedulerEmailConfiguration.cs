//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary
{
    using System;
    using System.Linq;
    using HR.TA.ServicePlatform.Configuration;

    /// <summary>
    /// Scheduler user configuration settings class
    /// </summary>
    [SettingsSection("SchedulerEmailConfiguration")]
    public class SchedulerEmailConfiguration
    {
        /// <summary>
        /// Gets or sets client url.
        /// </summary>
        public string ClientUrl { get; set; }

        /// <summary>
        /// Gets or sets the tenant id(s) (if more than one, they are semicolon separated) to bypass during Service Plan check
        /// </summary>
        public string EmailFooterBypassTenantIds { get; set; }

        /// <summary>
        /// Determine if the given tenant id should bypass additional email footer
        /// </summary>
        /// <param name="tenantId">tenant id to check</param>
        /// <returns>True if the given tenantId exists in EmailFooterBypassTenantIds</returns>
        public bool IsBypassTenant(string tenantId)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                return false;
            }

            return this.EmailFooterBypassTenantIds.Split(';').Any(t => t.Equals(tenantId, StringComparison.OrdinalIgnoreCase));
        }
    }
}
