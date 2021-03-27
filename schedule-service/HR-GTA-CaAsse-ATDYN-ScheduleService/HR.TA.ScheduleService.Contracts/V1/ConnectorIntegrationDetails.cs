//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using HR.TA.ScheduleService.Contracts.Enum;

namespace HR.TA.ScheduleService.Contracts.V1
{
    /// <summary>
    /// The Entity used to push message to Service bus Queue for communication with Connector Service
    /// </summary>
    public class ConnectorIntegrationDetails
    {
        /// <summary>
        /// Gets or sets Id of the Application
        /// </summary>
        public string JobApplicationId { get; set; }

        /// <summary>
        /// Gets or sets Id of the Schedule
        /// </summary>
        public string ScheduleID { get; set; }

        /// <summary>
        /// Gets or sets Profle Action
        /// </summary>
        public ActionType? ScheduleAction { get; set; }

        /// <summary>
        /// Gets or sets Root Activity Id
        /// </summary>
        public string RootActivityId { get; set; }
    }
}
