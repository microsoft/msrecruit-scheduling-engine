//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum JobOpeningVisibility
    {
        [EnumMember(Value = "internalOnly")]
        InternalOnly = 0,
        [EnumMember(Value = "public")]
        Public = 1
    }
}
