//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
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
