//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobPost.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.EntityExtensions
{
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Data.Utils;
    using System;
    using System.Collections.Generic;
    using System.Text;


    public static partial class EntityExtensions
    {
        /// <summary>The job post to  view model.</summary>
        /// <param name="jobPost">The job post.</param>
        /// <param name="jobDetailsUpdatedDateTime">The last updated timestamp of job opening.</param>
        /// <returns>The <see cref="ExternalJobPost"/>.</returns>
        public static ExternalJobPost ToViewModel(this JobPosting jobPost, DateTime? jobDetailsUpdatedDateTime)
        {
            if (jobPost != null)
            {
                var externalJobPost = new ExternalJobPost()
                {
                    Id = jobPost.RecId.ToString(),
                    Uri = jobPost.JobPostURI,
                    SupplierName = jobPost.JobPostSupplierName,
                    IsRepostAvailable = jobPost.PublicationStartDate < jobDetailsUpdatedDateTime
                };
                externalJobPost.ExtendedAttributes = new Dictionary<string, string>();

                if (jobPost.PublicationStartDate != null)
                {
                    externalJobPost.ExtendedAttributes.Add("publicationStartDate", jobPost.PublicationStartDate.Value.ToEpoch().ToString());
                }

                if (jobPost.PublicationCloseDate != null)
                {
                    externalJobPost.ExtendedAttributes.Add("publicationCloseDate", jobPost.PublicationCloseDate.Value.ToEpoch().ToString());
                }

                if (jobPost.Status != null)
                {
                    externalJobPost.ExtendedAttributes.Add("status", jobPost.Status.ToString());
                }

                return externalJobPost;
            }

            return null;
        }
    }
}
