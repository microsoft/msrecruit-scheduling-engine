//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA..Talent.EnumSetModel
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobApplicationActionType
    {
        [EnumMember(Value = "jobApplicationCreated")]
        JobApplicationCreated = 0,
        [EnumMember(Value = "sendToInterviewers")]
        SendToInterviewers = 1,
        [EnumMember(Value = "sendToCandidate")]
        SendToCandidate = 2,
    }
}
