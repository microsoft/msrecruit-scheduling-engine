//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum CandidateNoteType
    {
        [EnumMember(Value = "Application")]
        Application = 0,
        [EnumMember(Value = "Offer")]
        Offer = 1,
    }
}
