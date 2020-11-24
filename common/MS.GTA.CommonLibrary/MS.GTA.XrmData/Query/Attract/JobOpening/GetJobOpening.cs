//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="GetJobOpening.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.Query
{
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.XrmHttp;
    
    public partial class XrmQuery : IQuery
    {
        /// <summary>
        /// Gets the job opening internal asynchronous.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="jobOpeningId">The job opening identifier.</param>
        /// <param name="expand">The expand.</param>
        /// <returns>Job Opening</returns>
        private async Task<JobOpening> GetJobOpeningInternalAsync(IXrmHttpClient client, string jobOpeningId, Expression<Func<JobOpening, object>> expand = null)
        {
            var jobOpening = await client
                .Get(j => j.Autonumber == jobOpeningId, expand: expand)
                .ExecuteAndGetAsync("HcmAttXrmGetJobOpeningInternal");
            return jobOpening?.IsTemplate != true ? jobOpening : null;
        }
    }
}
