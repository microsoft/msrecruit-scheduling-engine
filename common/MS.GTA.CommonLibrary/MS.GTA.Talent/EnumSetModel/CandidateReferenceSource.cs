//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateReferenceSource.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
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
