//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    public enum TalentSource
    {
        [EnumMember(Value = "internal")]
        Internal = 0,
        [EnumMember(Value = "iCIMS")]
        ICIMS = 4
    }
}
