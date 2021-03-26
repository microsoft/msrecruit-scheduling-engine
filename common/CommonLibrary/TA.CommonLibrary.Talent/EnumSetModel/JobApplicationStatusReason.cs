//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum JobApplicationStatusReason
    {
        [EnumMember(Value = "new")]
        New = 0,
        [EnumMember(Value = "offerPrepared")]
        OfferPrepared = 1,
        [EnumMember(Value = "offerAccepted")]
        OfferAccepted = 2,
        [EnumMember(Value = "candidateWithdrew")]
        CandidateWithdrew = 3,
        [EnumMember(Value = "positionFilled")]
        PositionFilled = 4,
        [EnumMember(Value = "positionCancelled")]
        PositionCancelled = 5,
        [EnumMember(Value = "application")]
        Application = 6,
        [EnumMember(Value = "education")]
        Education = 7,
        [EnumMember(Value = "experience")]
        Experience = 8,
        [EnumMember(Value = "skillSet")]
        SkillSet = 9,
        [EnumMember(Value = "competency")]
        Competency = 10,
        [EnumMember(Value = "licensure")]
        Licensure = 11,
        [EnumMember(Value = "assessment")]
        Assessment = 12,
        [EnumMember(Value = "offerRejected")]
        OfferRejected = 13,
        [EnumMember(Value = "silverMedalist")]
        SilverMedalist = 14,
        // Microsoft specific status reasons, intentional start from 1001 as these might go away in near future
        [EnumMember(Value = "nicManagementExperience")]
        NicManagementExperience = 1001,
        [EnumMember(Value = "nicJobRelatedEducation")]
        NicJobRelatedEducation = 1002,
        [EnumMember(Value = "nicOther")]
        NicOther = 1003,
        [EnumMember(Value = "nicJobTechnicalFunctionalExperience")]
        NicJobTechnicalFunctionalExperience = 1004,
        [EnumMember(Value = "nicInconsistentJobHistory")]
        NicInconsistentJobHistory = 1005,
        [EnumMember(Value = "noRequiredQualification")]      
        NoRequiredQualification = 1006,
        [EnumMember(Value = "othersMoreQualified")]
        OthersMoreQualified = 1007
    }
}
