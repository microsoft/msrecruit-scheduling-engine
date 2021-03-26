//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum CandidateStatus
    {
        [EnumMember(Value = "available")]
        Available = 0,
        [EnumMember(Value = "notAvailable")]
        NotAvailable = 1
    }
}
