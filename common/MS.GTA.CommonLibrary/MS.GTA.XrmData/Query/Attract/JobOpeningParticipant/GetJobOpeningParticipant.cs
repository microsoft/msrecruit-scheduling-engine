//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="GetJobOpeningParticipant.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.Query
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.CommonDataService.Common.Internal;
    
    public partial class XrmQuery : IQuery
    {
        /// <summary>
        /// Get job opening participants by job Id
        /// </summary>
        /// <param name="jobOpeningId">Job opening id.</param>
        /// <returns>List of worker collection.</returns>
        public async Task<IEnumerable<JobOpeningParticipant>> GetJobOpeningParticipantByJobId(string jobOpeningId)
        {
            Contract.CheckNonEmpty(jobOpeningId, nameof(jobOpeningId));

            var client = await this.GetClient();
            var jobOpening = await this.GetJobOpeningInternalAsync(client, jobOpeningId);

            if (jobOpening == null)
            {
                throw new KeyNotFoundException($"Job Opening with jobOpeningId {jobOpeningId} not found ");
            }

            var batch = client.NewBatch();
            QueryJobOpeningParticipants(batch, jobOpening);
            await batch.ExecuteAsync("HcmAttXrmGetJOPByJobId");

            batch = client.NewBatch();
            QueryJobOpeningParticipantDelegates(batch, jobOpening.JobOpeningParticipants);
            await batch.ExecuteAsync("HcmAttXrmGetJOPByJobId2");

            return jobOpening.JobOpeningParticipants;
        }
    }
}
