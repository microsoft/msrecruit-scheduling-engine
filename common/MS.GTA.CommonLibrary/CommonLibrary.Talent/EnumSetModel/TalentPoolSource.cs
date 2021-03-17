//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract]
    public enum TalentPoolSource
    {
        [EnumMember(Value = "internal")]
        Internal = 0,
        [EnumMember(Value = "linkedIn")]
        LinkedIn = 2
    }
}
