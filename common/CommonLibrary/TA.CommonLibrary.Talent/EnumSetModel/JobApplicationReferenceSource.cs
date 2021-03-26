//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
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
