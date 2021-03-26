//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------


namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum FacetType
    {
        None = -1,
        CandidateName = 0,
        City = 1,
        Degree = 2,
        IsInternal = 3,
        Location = 4,
        Organization = 5,
        School = 6,
        Skills = 7,
        State = 8,
        TalentPools = 9,
        Title = 10,
        FieldOfStudy = 11,
        Rank = 12,
        Source = 13,
    }
}
