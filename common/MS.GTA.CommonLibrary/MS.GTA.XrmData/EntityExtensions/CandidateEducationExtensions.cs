//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateEducationExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static class CandidateEducationExtensions
    {
        public static Education ToViewModel(this CandidateEducation education) => education == null ? null : new Education()
        {
            EducationId = education.CandidateEducationID,
            School = education.School,
            Degree = education.Degree,
            FieldOfStudy = education.FieldOfStudy,
            Grade = education.Grade,
            Description = education.Description,
            FromMonthYear = education.FromMonthYear,
            ToMonthYear = education.ToMonthYear,
        };
    }
}
