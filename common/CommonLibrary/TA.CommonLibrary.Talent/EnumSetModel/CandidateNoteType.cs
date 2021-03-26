//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum CandidateNoteType
    {
        [EnumMember(Value = "Application")]
        Application = 0,
        [EnumMember(Value = "Offer")]
        Offer = 1,
    }
}
