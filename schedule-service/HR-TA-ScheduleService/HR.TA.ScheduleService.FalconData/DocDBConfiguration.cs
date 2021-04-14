//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.FalconData
{
    using HR.TA.Common.DocumentDB.Configuration;
    using HR.TA.ServicePlatform.Configuration;

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
