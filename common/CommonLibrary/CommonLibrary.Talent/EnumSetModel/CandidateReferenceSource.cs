//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum CandidateReferenceSource
    {
        [EnumMember(Value = "notSpecified")]
        NotSpecified = 0,

        [EnumMember(Value = "suggestedByCandidate")]
        SuggestedByCandidate = 1,

        [EnumMember(Value = "firstConnection")]
        FirstConnection = 2,

        [EnumMember(Value = "secondConnection")]
        SecondConnection = 3,

        [EnumMember(Value = "Other")]
        Other = 4
    }
}
