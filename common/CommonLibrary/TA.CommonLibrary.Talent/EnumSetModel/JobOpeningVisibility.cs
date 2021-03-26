//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum JobOpeningVisibility
    {
        [EnumMember(Value = "internalOnly")]
        InternalOnly = 0,
        [EnumMember(Value = "public")]
        Public = 1
    }
}
