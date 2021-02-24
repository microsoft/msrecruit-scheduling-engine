//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.ScheduleService.BusinessLibrary.Configurations
{
    using MS.GTA.ServicePlatform.Configuration;

    /// <summary>The IV client configuration.</summary>
    [SettingsSection("IVClientConfiguration")]
    public class IVClientConfiguration
    {
        /// <summary>Gets or sets the recruiting hub client URI.</summary>
        public string RecruitingHubClientUrl { get; set; }
    }
}
