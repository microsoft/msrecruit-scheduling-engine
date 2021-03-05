//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum CandidateNoteType
    {
        [EnumMember(Value = "Application")]
        Application = 0,
        [EnumMember(Value = "Offer")]
        Offer = 1,
    }
}
