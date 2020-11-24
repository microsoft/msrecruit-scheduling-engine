//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="StageActivityExtensions.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.ViewModelExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp.Util;
    using MS.GTA.TalentEntities.Enum;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static class StageActivityExtensions
    {
        /// <summary>The stage activity to job opening stage activity entity.</summary>
        /// <param name="stageActivity">The opening stage activity.</param>
        /// <param name="activityAudienceFeatureSetting">The feature setting for activity audience.</param>
        /// <returns>The <see cref="JobOpeningStageActivity"/>.</returns>
        public static JobOpeningStageActivity ToEntity(this StageActivity stageActivity, bool activityAudienceFeatureSetting) => stageActivity == null ? null : new JobOpeningStageActivity
        {
            ActivityType = stageActivity.ActivityType,
            Audience = stageActivity.Audience.GetValueOrDefault(stageActivity.ActivityType, ExtractForCandidate(stageActivity.Configuration), activityAudienceFeatureSetting),
            Configuration = stageActivity.Configuration,
            Description = stageActivity.Description,
            DisplayLabel = stageActivity.Name,
            IsEnabledForCandidate = stageActivity.IsEnableForCandidate,
            Ordinal = stageActivity.Ordinal,
            Required = stageActivity.Required,
            SubOrdinal = stageActivity.SubOrdinal,
            ODataUnmappedFields = stageActivity.GetCutomFields()
        };

        public static bool? ExtractForCandidate(string stageActivityConfiguration)
        {
            bool? forCandidate = null;
            if (stageActivityConfiguration != null)
            {
                forCandidate = JsonConvert.DeserializeObject<JobOpeningStageActivityConfiguration>(stageActivityConfiguration).ForCandidate;
            }
            return forCandidate;
        }

        public static ActivityAudience? GetValueOrDefault(this ActivityAudience? audience, JobApplicationActivityType? activityType, bool? forCandidate, bool activityAudienceFeature)
        {
            if (activityAudienceFeature == false && audience.HasValue == true)
            {
                if (forCandidate == null)
                {
                    audience = null;
                }
                else if (forCandidate == true)
                {
                    audience = ActivityAudience.AllCandidates;
                }
                else if (forCandidate == false)
                {
                    audience = ActivityAudience.HiringTeam;
                }
            }

            return GetValueOrDefault(audience, activityType, forCandidate);
        }

        public static ActivityAudience? GetValueOrDefault(this ActivityAudience? audience, JobApplicationActivityType? activityType, bool? forCandidate)
        {
            if (audience.HasValue)
            {
                return audience.Value;
            }

            switch (activityType)
            {
                case JobApplicationActivityType.Feedback:
                case JobApplicationActivityType.Interview:
                // case JobApplicationActivityType.Schedule:
                // case JobApplicationActivityType.Application:
                    return ActivityAudience.AllCandidates;

                default:
                    throw new ArgumentException($"The activity type {activityType} is not supported.", nameof(activityType));
            }
        }
    }
}
