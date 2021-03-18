//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum CandidateStatus
    {
        [EnumMember(Value = "available")]
        Available = 0,
        [EnumMember(Value = "notAvailable")]
        NotAvailable = 1
    }
}
