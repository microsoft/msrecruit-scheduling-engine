//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace TalentEntities.Enum
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum TalentPoolParticipantRole
    {
        [EnumMember(Value = "Owner")]
        Owner = 0,
        [EnumMember(Value = "Contributor")]
        Contributor = 1,
    }
}
