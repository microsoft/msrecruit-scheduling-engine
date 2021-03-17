//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum Required
    {   
        [EnumMember(Value = "forAll")]
        ForAll = 0,

        [EnumMember(Value = "forInternalOnly")]
        ForInternalyOnly = 1,

        [EnumMember(Value = "forExternalOnly")]
        ForExternalOnly = 2,

        [EnumMember(Value = "no")]
        No = 3,
    }
}