//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------


namespace MS.GTA.XrmData.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using MS.GTA.Common.Base.Helper;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.XrmHttp.Model;
    using MS.GTA.CommonDataService.Common.Internal;

    public partial class XrmQuery : IQuery
    {
        public static void LinkJobOpeningStageActivitiesAndJobOpeningParticipants(IList<JobApplicationActivity> activities, JobOpening jobOpening)
        {
            Contract.CheckValue(activities, nameof(activities));
            Contract.CheckValue(jobOpening, nameof(jobOpening));
            Contract.CheckValue(jobOpening.JobOpeningStages, nameof(jobOpening.JobOpeningStages));
            Contract.CheckValue(jobOpening.JobOpeningParticipants, nameof(jobOpening.JobOpeningParticipants));

            foreach (var activity in activities)
            {
                activity.JobOpeningStageActivity =
                    jobOpening.JobOpeningStages
                        .Where(s => s.JobOpeningStageActivities != null)
                        .SelectMany(s => s.JobOpeningStageActivities)
                        .FirstOrDefault(a => a.RecId == activity.JobOpeningStageActivityId);

                var jobOpeningStageId = activity.JobOpeningStageActivity?.JobOpeningStageId;
                if (jobOpeningStageId != null)
                {
                    activity.JobOpeningStageActivity.JobOpeningStage = jobOpening.JobOpeningStages.FirstOrDefault(s => s.RecId == jobOpeningStageId);
                }

                if (activity.JobApplicationActivityParticipants != null)
                {
                    foreach (var activityParticipant in activity.JobApplicationActivityParticipants)
                    {
                        activityParticipant.JobOpeningParticipant = jobOpening.JobOpeningParticipants.FirstOrDefault(p => p.RecId == activityParticipant.JobOpeningParticipantId);
                    }
                }
            }
        }

        public async Task<JobApplication> GetJobApplicationMetadata(string jobApplicationId, bool useAdminClient = false)
        {
            Contract.CheckNonEmpty(jobApplicationId, nameof(jobApplicationId));
            return await this.GetJobApplicationUsingAK(a => a.Autonumber == jobApplicationId, null, useAdminClient);
        }

        public async Task<JobApplication> GetJobApplicationReference(string jobApplicationId, bool useAdminClient = false)
        {
            Contract.CheckNonEmpty(jobApplicationId, nameof(jobApplicationId));
            IXrmHttpClient client = null;
            if (useAdminClient)
            {
                client = await this.GetAdminClient();
            }
            else
            {
                client = await this.GetClient();
            }

            return await client.Get<JobApplication>(a => a.Autonumber == jobApplicationId).ExecuteAndGetAsync("HcmAttXrmGetJobApplicationRef");
        }

        /// <summary>
        /// Get a job application by ID
        /// </summary>
        /// <param name="jobApplicationId">Job application Id</param>
        /// <param name="useAdminClient">true if the admin client should be used explicitly, otherwise false; optional</param>
        /// <returns>The job application entity.</returns>
        public async Task<JobApplication> GetJobApplicationWithDetails(string jobApplicationId, bool useAdminClient = false)
        {
            Contract.CheckNonEmpty(jobApplicationId, nameof(jobApplicationId));

            var client = useAdminClient ? await this.GetAdminClient() : await this.GetClient();
            return await this.GetApplicationWithDetailsUsingFilter(client, a => a.Autonumber == jobApplicationId);
        }

        #region Private Methods
        private async Task<JobApplication> GetJobApplicationUsingAK(Expression<Func<JobApplication, bool>> filter = null, Expression<Func<JobApplication, object>> expand = null, bool useAdminMode = false)
        {
            var client = useAdminMode ? await this.GetAdminClient() : await this.GetClient();
            return await client
                .Get<JobApplication>(
                    filter,
                    expand: expand)
                .ExecuteAndGetAsync("HcmAttXrmGetJobAppInternal");
        }

        private async Task<JobApplication> GetApplicationWithDetailsUsingFilter(IXrmHttpClient client, Expression<Func<JobApplication, bool>> filter)
        {
            var applications = await client
                .GetAll(filter, expand: a => new { a.JobOpening, a.JobPosition })
                .ExecuteAndGetAsync("HcmAttXrmJobApplicationGet1");

            var application = applications.Result?.FirstOrDefault();
            if (application?.JobOpening == null)
            {
                return null;
            }

            var batch = client.NewBatch();
            QueryJobApplicationCandidate(batch, application);

            QueryJobApplicationAndProfileCandidateArtifacts(batch, application);

            QueryJobApplicationActivities(batch, application);
            QueryJobApplicationComments(batch, application);
            QueryJobOpeningParticipants(batch, application.JobOpening);
            QueryJobOpeningStages(batch, application.JobOpening);
            QueryJobPostings(batch, application.JobOpening);

            await batch.ExecuteAsync("HcmAttXrmJobApplicationGet2");

            batch = client.NewBatch();
            var adminClient = await this.GetAdminClient();
            var adminBatch = adminClient.NewBatch();


            QueryTalentSource(batch, application);
            QueryJobOpeningParticipantDelegates(batch, application.JobOpening.JobOpeningParticipants);
            QueryJobApplicationActivityParticipants(batch, application.JobApplicationActivities);
            QueryJobApplicationActivityAvailabilities(batch, application.JobApplicationActivities);
            QueryJobOpeningStageActivities(batch, application.JobOpening.JobOpeningStages);
            QueryCandidateTrackingTalentTagAssociations(batch, application.Candidate.Trackings);
            if (application.CandidateArtifacts != null)
            {
                this.Trace.TraceInformation($"Getting creator for {application.CandidateArtifacts.Count} artifacts");
                QueryArtifactCreators(batch, application.CandidateArtifacts);
            }

            FilterCandidateArtifactWithoutAnnotations(adminBatch, application);

            await Task.WhenAll(
                batch.ExecuteAsync("HcmAttXrmJobApplicationGet3"),
                adminBatch.ExecuteAsync("HcmAttXrmJobApplicationGetAdmin"));

            batch = client.NewBatch();
            QueryJobApplicationActivityParticipantMeetings(batch, application.JobApplicationActivities.SelectMany(a => a.JobApplicationActivityParticipants).ToList());
            QueryTalentTagAssociationTags(batch, application.Candidate.Trackings.SelectMany(t => t.TalentTagAssociations).ToArray());
            await batch.ExecuteAsync("HcmAttXrmJobApplicationGet4");

            // Rearrange entities for easier translation to client contracts

            application.CurrentJobOpeningStage = application.JobOpening.JobOpeningStages.FirstOrDefault(s => s.RecId == application.CurrentJobOpeningStageId);

            LinkJobOpeningStageActivitiesAndJobOpeningParticipants(application.JobApplicationActivities, application.JobOpening);

            foreach (var comment in application.JobApplicationComments)
            {
                var participant = application.JobOpening.JobOpeningParticipants.FirstOrDefault(p => p.RecId == comment.JobOpeningParticipantId);
                if (participant == null)
                {
                    var workerFound = await client.Get<Worker>(c => c.SystemUserId == comment.XrmCreatedById).ExecuteAndGetAsync("HcmAttXrmAttachGetWorker");
                    participant = new JobOpeningParticipant() { Worker = workerFound };
                }
                comment.JobOpeningParticipant = participant;
            }

            application.Candidate.CandidateArtifacts = application.CandidateArtifacts;
            return application;
        }

        private static void QueryJobApplicationCandidate(IXrmHttpClientBatch batch, JobApplication application)
        {
            batch.Add(
                batch.Client.Get<Candidate>(application.CandidateId.Value, expand: c => new { c.CandidateEducations, c.CandidateWorkExperiences, c.CandidateSkills, c.Trackings })
                .Expand(c => c.Worker, w => new { w.RecId, w.OfficeGraphIdentifier }),
                r => application.Candidate = r);
        }

        private static void QueryJobApplicationAndProfileCandidateArtifacts(IXrmHttpClientBatch batch, JobApplication application)
        {
            batch.Add(
                batch.Client.GetAll<CandidateArtifact>(a => a.JobApplicationId == application.RecId || (a.CandidateId == application.CandidateId && a.JobApplication == null)),
                r => application.CandidateArtifacts = r.Result);
        }

        private static void QueryJobApplicationActivities(IXrmHttpClientBatch batch, JobApplication application)
        {
            batch.AddWithFinishMultipage(
                batch.Client.GetAll<JobApplicationActivity>(a => a.JobApplicationId == application.RecId),
                r => application.JobApplicationActivities = r);
        }

        private static void QueryJobApplicationComments(IXrmHttpClientBatch batch, JobApplication application)
        {
            batch.AddWithFinishMultipage(
                batch.Client.GetAll<JobApplicationComment>(a => a.JobApplicationId == application.RecId),
                r => application.JobApplicationComments = r);
        }

        private static void QueryJobOpeningParticipants(IXrmHttpClientBatch batch, JobOpening jobOpening)
        {
            batch.AddWithFinishMultipage(
                batch.Client.GetAll<JobOpeningParticipant>(p => p.JobOpeningId == jobOpening.RecId, expand: p => new { p.Worker }),
                ps => jobOpening.JobOpeningParticipants = ps);
        }
        private static void QueryJobOpeningStages(IXrmHttpClientBatch batch, JobOpening jobOpening)
        {
            QueryJobOpeningStages(batch, new[] { jobOpening });
        }

        private static void QueryJobOpeningStages(IXrmHttpClientBatch batch, IList<JobOpening> jobOpenings)
        {
            batch.QueryExpandAllForList<JobOpening, JobOpeningStage>(jobOpenings, p => p.RecId, (p, c) => p.JobOpeningStages = c, c => c.JobOpeningId);
        }
        private static void QueryJobPostings(IXrmHttpClientBatch batch, JobOpening jobOpening)
        {
            QueryJobPostings(batch, new[] { jobOpening });
        }

        private static void QueryJobPostings(IXrmHttpClientBatch batch, IList<JobOpening> jobOpenings)
        {
            batch.QueryExpandAllForList<JobOpening, JobPosting>(jobOpenings, p => p.RecId, (p, c) => p.JobPostings = c, c => c.JobOpeningId);
        }

        private static void QueryTalentSource(IXrmHttpClientBatch batch, JobApplication application)
        {
            batch.AddWithFinishMultipage(
                batch.Client.GetAll<TalentSourceDetail>(tsd => tsd.JobApplicationId == application.RecId, expand: tsd => new { tsd.TalentSource, tsd.Worker }),
                r => application.TalentSourceDetails = r);

            if (application?.Candidate?.RecId != null)
            {
                batch.AddWithFinishMultipage(
                    batch.Client.GetAll<TalentSourceDetail>(tsd => tsd.CandidateId == application.Candidate.RecId, expand: tsd => new { tsd.TalentSource, tsd.Worker }),
                    r => application.Candidate.TalentSourceDetails = r);
            }
        }

        private static void QueryJobOpeningParticipantDelegates(IXrmHttpClientBatch batch, IList<JobOpeningParticipant> participants)
        {
            // TODO: generalize and pull out into a helper method.
            if (participants != null && participants.Any())
            {
                foreach (var chunk in participants.Chunk(30))
                {
                    var query = new FetchXmlQuery<Worker>(selectAllFields: true);
                    query.AddInnerJoin<JobOpeningParticipantDelegate>(
                        w => w.RecId,
                        a => a.WorkerId,
                        a => chunk.Select(p => p.RecId).Contains(a.JobOpeningParticipantId),
                        alias: "P",
                        selectAllFields: true);
                    batch.Add(
                        batch.Client.GetAllWithFetchXml<Worker, Worker>(query),
                        r =>
                        {
                            foreach (var participant in chunk)
                            {
                                participant.Delegates =
                                    r.Result
                                        .Where(w => w.GetJoinedFetchXmlRecord<JobOpeningParticipantDelegate>("P").JobOpeningParticipantId == participant.RecId)
                                        .ToArray();
                            }
                        });
                }
            }
        }

        private static void QueryJobApplicationActivityParticipants(IXrmHttpClientBatch batch, IList<JobApplicationActivity> jobApplicationActivities)
        {
            batch.QueryExpandAllForList<JobApplicationActivity, JobApplicationActivityParticipant>(jobApplicationActivities, p => p.RecId, (p, c) => p.JobApplicationActivityParticipants = c, c => c.JobApplicationActivityId);
        }

        private static void QueryJobApplicationActivityAvailabilities(IXrmHttpClientBatch batch, IList<JobApplicationActivity> jobApplicationActivities)
        {
            batch.QueryExpandAllForList<JobApplicationActivity, JobApplicationActivityAvailability>(jobApplicationActivities, p => p.RecId, (p, c) => p.JobApplicationActivityAvailabilities = c, c => c.JobApplicationActivityId);
        }

        private static void QueryJobOpeningStageActivities(IXrmHttpClientBatch batch, IList<JobOpeningStage> jobOpeningStages)
        {
            batch.QueryExpandAllForList<JobOpeningStage, JobOpeningStageActivity>(jobOpeningStages, p => p.RecId, (p, c) => p.JobOpeningStageActivities = c, c => c.JobOpeningStageId);
        }

        private static void QueryCandidateTrackingTalentTagAssociations(IXrmHttpClientBatch batch, IList<CandidateTracking> trackings)
        {
            batch.QueryExpandAllForList<CandidateTracking, TalentTagAssociation>(trackings, p => p.RecId, (p, c) => p.TalentTagAssociations = c, c => c.CandidateTrackingId);
        }

        private static void QueryArtifactCreators(IXrmHttpClientBatch batch, IList<CandidateArtifact> artifacts)
        {
            batch.QueryExpandForList<CandidateArtifact, SystemUser>(artifacts, p => p.XrmCreatedById, (p, c) => p.XrmCreatedBy = c, c => c.SystemUserId);
        }

        private static void FilterCandidateArtifactWithoutAnnotations(IXrmHttpClientBatch batch, JobApplication application)
        {
            var ids = application.CandidateArtifacts.Select(x => x.RecId.Value).ToList();
            if (ids.Any())
            {
                batch.Add(
                    batch.Client.GetAll<Annotation>(a => ids.Contains(a.RelatedEntityRecId.Value), select: a => new { a.RecId, a.RelatedEntityRecId }),
                    r =>
                    {
                        var relatedEntityIds = r.Result?.Select(x => x.RelatedEntityRecId.Value).ToList();
                        application.CandidateArtifacts = relatedEntityIds.Any() ?
                            application.CandidateArtifacts.Where(x => relatedEntityIds.Contains(x.RecId.Value)).ToList() :
                            new List<CandidateArtifact>();
                    });
            }
        }

        private static void QueryJobApplicationActivityParticipantMeetings(IXrmHttpClientBatch batch, IList<JobApplicationActivityParticipant> jobApplicationActivityParticipants)
        {
            batch.QueryExpandAllForList<JobApplicationActivityParticipant, JobApplicationActivityParticipantMeeting>(jobApplicationActivityParticipants, p => p.RecId, (p, c) => p.JobApplicationActivityParticipantMeetings = c, c => c.JobApplicationActivityParticipantId);
        }

        private static void QueryTalentTagAssociationTags(IXrmHttpClientBatch batch, IList<TalentTagAssociation> tagAssociations)
        {
            batch.QueryExpandForList<TalentTagAssociation, TalentTag>(tagAssociations, p => p.TalentTagId, (p, c) => p.TalentTag = c, c => c.RecId);
        }

        #endregion
    }
}
