//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum CandidateStatus
    {
        [EnumMember(Value = "available")]
        Available = 0,
        [EnumMember(Value = "notAvailable")]
        NotAvailable = 1
    }
}
