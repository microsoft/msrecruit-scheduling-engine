//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
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
