//----------------------------------------------------------------------------
// <copyright file="DocDBConfiguration.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.FalconData
{
    using MS.GTA.Common.DocumentDB.Configuration;
    using MS.GTA.ServicePlatform.Configuration;

    /// <summary>
    /// DocDB staorage configuration
    /// </summary>
    [SettingsSection("DocumentDBStorageConfiguration")]
    public class DocDBConfiguration : DocumentDBStorageConfiguration
    {
        /// <summary>
        /// Gets or sets gTA common container id
        /// </summary>
        public string CommonContainerId { get; set; }

        /// <summary>
        /// Gets or sets gTA IV Schedule container id
        /// </summary>
        public string IVScheduleContainerId { get; set; }

        /// <summary>
        /// Gets or sets gTA IV Schedule container id
        /// </summary>
        public string IVScheduleHistoryContainerID { get; set; }
    }
}
