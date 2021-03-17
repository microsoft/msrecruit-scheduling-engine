//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum TalentPoolParticipantRole
    {
        [EnumMember(Value = "Owner")]
        Owner = 0,
        [EnumMember(Value = "Contributor")]
        Contributor = 1,
    }
}
