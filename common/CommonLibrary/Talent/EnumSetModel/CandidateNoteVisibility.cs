//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum CandidateNoteVisibility
    {
        [EnumMember(Value = "private")]
        Private = 0,
        [EnumMember(Value = "public")]
        Public = 1
    }
}
