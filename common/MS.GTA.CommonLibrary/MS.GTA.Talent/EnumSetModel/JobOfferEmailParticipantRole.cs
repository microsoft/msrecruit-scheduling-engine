//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEntities.Enum
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobOfferEmailParticipantRole
    {
        [EnumMember(Value = "hiringManager")]
        HiringManager = 0,
        [EnumMember(Value = "recruiter")]
        Recruiter = 1,
        [EnumMember(Value = "interviewer")]
        Interviewer = 2,
        [EnumMember(Value = "contributor")]
        Contributor = 3,
        [EnumMember(Value = "offerCreator")]
        OfferCreator = 4,
        [EnumMember(Value = "offerRejector")]
        OfferRejector = 5,
    }
}
