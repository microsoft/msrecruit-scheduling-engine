//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum CandidateEthnicOrigin
    {
        [EnumMember(Value = "notSpecified")]
        NotSpecified = 0,

        [EnumMember(Value = "AfricanAmericanOrBlack")]
        AfricanAmericanOrBlack = 1,

        [EnumMember(Value = "AmericanIndiaOrAlaskaNative")]
        AmericanIndianOrAlaskaNative = 2,

        [EnumMember(Value = "Asian")]
        Asian = 3,

        [EnumMember(Value = "CaucasianOrWhile")]
        CaucasianOrWhile = 4,

        [EnumMember(Value = "HispanicOrLatino")]
        HispanicOrLatino = 5,

        [EnumMember(Value = "MultiRacial")]
        MultiRacial = 6,

        [EnumMember(Value = "NativeHawaiianOrOtherPacificIslander")]
        NativeHawaiianOrOtherPacificIslander = 7,

        [EnumMember(Value = "doNotWantToAnswer")]
        DoNotWantToAnswer = 8
    }
}
