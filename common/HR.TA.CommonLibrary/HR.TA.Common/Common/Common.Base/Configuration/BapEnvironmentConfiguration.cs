﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using HR.TA.ServicePlatform.Configuration;

namespace HR.TA.Common.Cdm.Configuration
{
    /// <summary>
    /// CDM Configuration
    /// </summary>
    [SettingsSection(nameof(BapEnvironmentConfiguration))]
    public class BapEnvironmentConfiguration
    {
        /// <summary>
        /// Gets or sets Tenant Object Id
        /// </summary>
        public string BapEnvironmentString { get; set; }

        /// <summary>
        /// Get environment mapping
        /// </summary>
        /// <returns> Dictionary of tenant and environment.</returns>
        public IDictionary<string, string> GetEnvironmentMapping()
        {
            var tenants = this.BapEnvironmentString.Split(';');
            IDictionary<string, string> environmentMapping = new Dictionary<string, string>();
            foreach (var tenant in tenants)
            {
                var environmentInfo = tenant.Split('|');
                if (environmentInfo != null && environmentInfo.Length == 3 && !environmentMapping.ContainsKey(environmentInfo[1]))
                {
                    environmentMapping.Add(environmentInfo[1], environmentInfo[2]);
                }
            }

            return environmentMapping;
        }
    }
}