//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum CandidateNoteType
    {
        [EnumMember(Value = "Application")]
        Application = 0,
        [EnumMember(Value = "Offer")]
        Offer = 1,
    }
}
