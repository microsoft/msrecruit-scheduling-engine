//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum CandidateNoteVisibility
    {
        [EnumMember(Value = "private")]
        Private = 0,
        [EnumMember(Value = "public")]
        Public = 1
    }
}
