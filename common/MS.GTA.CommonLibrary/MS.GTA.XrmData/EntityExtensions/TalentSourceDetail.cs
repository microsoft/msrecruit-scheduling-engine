//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TalentSourceDetail.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.XrmData.Query.Attract.OptionSetMetadata;
    using System;
    using System.Collections.Generic;
    using System.Text;


    /// <summary>The entity extensions.</summary>
    public static partial class EntityExtensions
    {
        /// <summary>The talent source detail to  view model.</summary>
        /// <param name="talentSourceDetail">The talent source detail.</param>
        /// <param name="optionSetInfo">Option set info.</param>
        /// <param name="talentSourceCategoryValue">Talent source category value</param>
        /// <returns>The <see cref="TalentSource"/>.</returns>
        public static TalentSource ToViewModel(this TalentSourceDetail talentSourceDetail, OptionSetInfo optionSetInfo = null, OptionSetValue talentSourceCategoryValue = null)
        {
            if (talentSourceDetail != null)
            {
                var talentSource = new TalentSource()
                {
                    Id = talentSourceDetail.TalentSourceId?.ToString(),
                    Name = talentSourceDetail.TalentSource?.Name,
                    Domain = talentSourceDetail.TalentSource?.Domain,
                    TalentSourceCategory = talentSourceCategoryValue != null ? talentSourceCategoryValue : GetTalentSourceCategory(talentSourceDetail.TalentSource, optionSetInfo),
                    ReferalReference = talentSourceDetail.Worker?.ToViewModel()
                };

                return talentSource;
            }

            return null;
        }

        private static OptionSetValue GetTalentSourceCategory(TalentSourceEntity talentSourceEntity, OptionSetInfo optionSetInfo)
        {
            return optionSetInfo?.GetOptionSetValue(talentSourceEntity, j => j.TalentSourceCategory);
        }
    }
}
