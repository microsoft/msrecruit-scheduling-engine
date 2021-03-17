//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum CandidateTrackingCategory
    {
        [EnumMember(Value = "futureInterest")]
        FutureInterest = 0
    }
}
