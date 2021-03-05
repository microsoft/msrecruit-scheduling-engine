//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum AssessmentProvider
    {
        [EnumMember(Value = "default")]
        Default = 0,

        [EnumMember(Value = "koru")]
        Koru = 1 
    }
}
