//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum AssessmentProvider
    {
        [EnumMember(Value = "default")]
        Default = 0,

        [EnumMember(Value = "koru")]
        Koru = 1 
    }
}
