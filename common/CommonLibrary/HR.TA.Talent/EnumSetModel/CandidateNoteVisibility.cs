//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum CandidateNoteVisibility
    {
        [EnumMember(Value = "private")]
        Private = 0,
        [EnumMember(Value = "public")]
        Public = 1
    }
}
