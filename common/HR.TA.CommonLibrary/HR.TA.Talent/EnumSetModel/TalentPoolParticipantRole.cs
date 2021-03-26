//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace HR.TA.TalentEntities.Enum
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum TalentPoolParticipantRole
    {
        [EnumMember(Value = "Owner")]
        Owner = 0,
        [EnumMember(Value = "Contributor")]
        Contributor = 1,
    }
}
