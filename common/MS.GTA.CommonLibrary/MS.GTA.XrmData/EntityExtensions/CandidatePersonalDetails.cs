//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidatePersonalDetails.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp.Util;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Model = MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;

    public static partial class ViewModelExtensions
    {
        /// <summary>The candidate personal details to view models.</summary>
        /// <param name="details">The applicant.</param>
        /// <returns>The <see cref="Candidate"/>.</returns>
        public static CandidatePersonalDetails ToViewModel(this Model.CandidatePersonalDetails details)
        {
            if (details != null)
            {
                return new CandidatePersonalDetails()
                {
                    Gender = details.Gender,
                    MilitaryStatus = details.MilitaryStatus,
                    VeteranStatus = details.VeteranStatus,
                    Ethnicity = details.Ethnicity,
                    DisabilityStatus = details.DisabilityStatus,
                    SubmittedOn = details.XrmModifiedOn.HasValue ? details.XrmModifiedOn : details.XrmCreatedOn,
                    JobApplicationActivityId = details.JobApplicationActivity?.Autonumber,
                    CustomFields = details.GetCustomFields()
                };
            }

            return null;
        }
    }
}
