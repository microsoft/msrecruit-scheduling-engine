//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum JobApplicationReferenceSource
    {
        [EnumMember(Value = "notSpecified")]
        NotSpecified = 0,
        [EnumMember(Value = "linkedIn")]
        LinkedIn = 1,
        [EnumMember(Value = "facebook")]
        Facebook = 2,
        [EnumMember(Value = "candidate")]
        Candidate = 3
    }
}
