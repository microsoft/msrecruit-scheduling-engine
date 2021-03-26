//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Talent.EnumSetModel
{
     using System.Runtime.Serialization;

    [DataContract]
    public enum ScheduleStatus
    {
        [EnumMember(Value = "notScheduled")]
        NotScheduled = 0,
        [EnumMember(Value = "saved")]
        Saved = 1,
        [EnumMember(Value = "queued")]
        Queued = 2,
        [EnumMember(Value = "sent")]
        Sent = 3,
        [EnumMember(Value = "failedToSend")]
        FailedToSend = 4,
        [EnumMember(Value = "delete")]
        Delete
    }
}
