//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum EntityType
    {
        [EnumMember(Value = "jobOpeningPosition")]
        JobOpeningPosition = 0,
        [EnumMember(Value = "jobPost")]
        JobPost = 1,
        [EnumMember(Value = "candidate")]
        Candidate = 2,
        [EnumMember(Value = "jobOpening")]
        JobOpening = 3,
        [EnumMember(Value = "jobApplication")]
        JobApplication = 4
    }
}
