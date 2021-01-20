// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EntityType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
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
