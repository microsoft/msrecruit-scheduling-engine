//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateTrackingExtensions.cs">
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
    using System.Linq;
    using System.Text;


    public static class CandidateTrackingExtensions
    {
        public static ApplicantTagTracking ToViewModel(this CandidateTracking candidateTracking) => candidateTracking == null ? null : new ApplicantTagTracking()
        {
            Id = candidateTracking.RecId.Value.ToString(),
            Category = candidateTracking.TrackingCategory.GetValueOrDefault(),
            Owner = new Person()
            {
                GivenName = candidateTracking.XrmOwningUser?.FirstName,
                MiddleName = candidateTracking.XrmOwningUser?.MiddleName,
                Surname = candidateTracking.XrmOwningUser?.LastName,
                Email = candidateTracking.XrmOwningUser?.PrimaryEmail,
            },
            Tags = candidateTracking.TalentTagAssociations?.Where(tta => tta != null).Select(tta => tta.ToViewModel()).ToList(),
        };
    }
}
