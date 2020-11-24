//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum AssessmentProvider
    {
        [EnumMember(Value = "default")]
        Default = 0,

        [EnumMember(Value = "koru")]
        Koru = 1 
    }
}
