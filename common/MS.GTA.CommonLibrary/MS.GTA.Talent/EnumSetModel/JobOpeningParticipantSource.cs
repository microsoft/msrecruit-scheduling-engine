//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobOpeningParticipantSource
    {
        [EnumMember(Value = "Internal")]
        Internal = 0,
        [EnumMember(Value = "MSDataMall")]
        MSDataMall = 1,
        [EnumMember(Value = "LinkedIn")]
        LinkedIn = 2,
        [EnumMember(Value = "Greenhouse")]
        Greenhouse = 3,
        [EnumMember(Value = "iCIMS")]
        ICIMS = 4
    }
}
