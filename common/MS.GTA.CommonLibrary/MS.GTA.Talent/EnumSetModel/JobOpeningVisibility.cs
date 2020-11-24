//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobOpeningVisibility
    {
        [EnumMember(Value = "internalOnly")]
        InternalOnly = 0,
        [EnumMember(Value = "public")]
        Public = 1
    }
}
