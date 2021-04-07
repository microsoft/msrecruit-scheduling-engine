//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace HR.TA.ScheduleWorker
{
    public class ConnectorIntegrationDetails
    {
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

    /// <summary>
    /// Contract for Referral Action Type
    /// </summary>
    [DataContract]
    public enum ActionType
    {
        /// <summary>Create</summary>
        Create,

        /// <summary>Update</summary>
        Update,

        /// <summary>Delete</summary>
        Delete,
    }
}
