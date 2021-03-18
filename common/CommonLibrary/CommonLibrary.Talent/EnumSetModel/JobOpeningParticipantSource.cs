//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
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
