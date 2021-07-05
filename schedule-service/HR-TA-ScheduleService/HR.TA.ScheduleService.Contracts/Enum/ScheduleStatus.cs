//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Talent.EnumSetModel
{
     using System.Runtime.Serialization;

    /// <summary>
    /// Schedule status
    /// </summary>
    [DataContract]
    public enum ScheduleStatus
    {
        /// <summary>
        /// NotScheduled
        /// </summary>
        [EnumMember(Value = "notScheduled")]
        NotScheduled = 0,
        /// <summary>
        /// Saved
        /// </summary>
        [EnumMember(Value = "saved")]
        Saved = 1,
        /// <summary>
        /// Queued
        /// </summary>
        [EnumMember(Value = "queued")]
        Queued = 2,
        /// <summary>
        /// Sent
        /// </summary>
        [EnumMember(Value = "sent")]
        Sent = 3,
        /// <summary>
        /// FailedToSend
        /// </summary>
        [EnumMember(Value = "failedToSend")]
        FailedToSend = 4,
        /// <summary>
        /// Delete
        /// </summary>
        [EnumMember(Value = "delete")]
        Delete
    }
}
