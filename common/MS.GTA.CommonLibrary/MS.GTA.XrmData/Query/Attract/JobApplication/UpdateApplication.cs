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

    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.TalentEntities.Enum;
    using MS.GTA.XrmData.Query.ViewModelExtensions;

    public partial class XrmQuery : IQuery
    {
        /// <summary>
        /// Remove position from applicaition after application gets closed
        /// </summary>
        /// <param name="jobApplicationId">Job application Id</param>
        /// <param name="useAdminClient">true if the admin client should be used explicitly, otherwise false; optional</param>
        /// <returns>Whether the delete position from application was successful</returns>
        public async Task<bool> RemovePositionFromApplication(string jobApplicationId, bool useAdminClient = false)
        {
            IXrmHttpClient client = null;
            if (useAdminClient)
            {
                client = await this.GetAdminClient();
            }
            else
            {
                client = await this.GetClient();
            }

            var jobApplicationRecord = await this.GetJobApplicationReference(jobApplicationId, useAdminClient);
            if (jobApplicationRecord == null)
            {
                throw new Exception($"Job Application with Job Application Id {jobApplicationId} not found");
            }

            if (jobApplicationRecord.JobPositionId != null)
            {
                await client.Dissociate<JobPosition, JobApplication>(jobApplicationRecord.JobPositionId.Value, jobApplicationRecord.RecId.Value, position => position.JobApplications)
                    .ExecuteAsync("HcmAttXrmUpdateApplicationPosition");
            }

            return true;
        }

        /// <summary>
        /// Update the job application status reason
        /// </summary>
        /// <param name="jobApplicationId">Job application Id</param>
        /// <param name="statusReason">The reason to update</param>
        /// <param name="useAdminClient">true if the admin client should be used explicitly, otherwise false; optional</param>
        /// <returns>Whether the operation was successful</returns>
        public async Task<bool> UpdateJobApplicationStatusReason(string jobApplicationId, JobApplicationStatusReason statusReason, bool useAdminClient = false)
        {
            Contract.CheckNonEmpty(jobApplicationId, nameof(jobApplicationId));
            var trace = $"Update job application status reason of {jobApplicationId} to {statusReason}";
            this.Trace.TraceInformation(trace);

            IXrmHttpClient client = null;
            if (useAdminClient)
            {
                client = await this.GetAdminClient();
            }
            else
            {
                client = await this.GetClient();
            }

            // Retrieve the job application first
            var application = await client.Get<JobApplication>(a => a.Autonumber == jobApplicationId).ExecuteAndGetAsync("HcmAttXrmUpdAppGet");
            if (application == default(JobApplication))
            {
                throw new Exception($"Job Application with Job Application Id {jobApplicationId} not found");
            }

            var batch = client.NewBatch();
            var changeset = batch.AddNewChangeSet();

            // TODO: dakahn - Remove after full rollout of ServiceBus support. All history updates will be done by ServiceBus
            // TODO: RSA - Check if XRM Service bus listener is needed or not for GTA.
            ////if (!XrmServiceBusListenerFlight.IsEnabled || !XrmServiceBusMiscFlight.IsEnabled)
            ////{
            ////    // Create job application history
            ////    changeset.Add(client.Create(new JobApplicationHistory()
            ////    {
            ////        Status = application.Status,
            ////        StatusReason = statusReason,
            ////        TransitionDateTime = DateTime.UtcNow,
            ////        JobApplication = application,
            ////    }));
            ////}

            application.StatusReason = statusReason;

            // setting the offer accepted date time in job application
            if (statusReason == JobApplicationStatusReason.OfferAccepted)
            {
                application.OfferAcceptDate = DateTime.UtcNow;
            }
            else
            {
                application.OfferAcceptDate = null;
            }
            changeset.Add(UpdateApplicationRecord(client, application, a => new { a.StatusReason, a.OfferAcceptDate }));

            await batch.ExecuteAsync("HcmAttXrmUpdAppStRsn");

            this.Trace.TraceInformation(trace + "executed");
            return true;
        }

        /// <summary>
        /// Add or delete team members for a job application activity.
        /// </summary>
        /// <param name="teamMembers">Team members.</param>
        /// <param name="jobApplicationId">Job application id.</param>
        /// <param name="activityId">Application Activity id.</param>
        /// <returns>Task performed.</returns>
        /// <returns>True or false.</returns>
        public async Task UpdateJobApplicationActivityTeamMembers(IList<HiringTeamMember> teamMembers, string jobApplicationId, string activityId)
        {
            Contract.CheckNonEmpty(activityId, nameof(activityId));
            Contract.CheckNonEmpty(jobApplicationId, nameof(jobApplicationId));
            Contract.CheckValue(teamMembers, nameof(teamMembers));

            /* TODO: replace with getClient*/
            var client = await this.GetAdminClient();

            var jobApplication = await this.GetRelatedJobApplicationActivitiesById(activityId);

            if (jobApplication?.Autonumber != jobApplicationId)
            {
                throw new Exception($"Job Application Activity: {activityId}  not exist in the system.");
            }

            var jobOpening = jobApplication?.JobOpening;
            if (jobOpening?.JobOpeningParticipants == null)
            {
                throw new Exception($"Job Application Activity: {activityId}  not exist in the system.");
            }

            var teamMemberInfo = teamMembers.Where(t => t != null).Select(teamMember => new { teamMember.UserAction, teamMember.Ordinal, jobOpeningParticipant = teamMember.ToEntity() }).ToArray();
            await this.FindOrCreateWorkers(client, teamMemberInfo.Select(t => t.jobOpeningParticipant?.Worker).ToArray());

            var batch = client.NewBatch();

            foreach (var teamMember in teamMemberInfo.Where(t => t.UserAction == UserAction.Delete))
            {
                var worker = teamMember.jobOpeningParticipant.Worker;
                foreach (var relatedActivity in jobApplication.JobApplicationActivities)
                {
                    var activityParticipant = relatedActivity.JobApplicationActivityParticipants.FirstOrDefault(p => p.JobOpeningParticipant?.WorkerId == worker.RecId);
                    if (activityParticipant != null)
                    {
                        batch.Add(client.Delete<JobApplicationActivityParticipant>(activityParticipant.RecId.Value));
                    }
                }
            }

            foreach (var teamMember in teamMemberInfo.Where(t => t.UserAction == UserAction.Add || t.UserAction == UserAction.Update))
            {
                var worker = teamMember.jobOpeningParticipant.Worker;
                var jobParticipant = jobOpening.JobOpeningParticipants.FirstOrDefault(p => p.WorkerId == worker.RecId);
                if (jobParticipant == null)
                {
                    teamMember.jobOpeningParticipant.JobOpening = jobOpening;
                    batch.Add(client.Create(teamMember.jobOpeningParticipant), r => teamMember.jobOpeningParticipant.RecId = r);
                }
                else
                {
                    teamMember.jobOpeningParticipant.RecId = jobParticipant.RecId;
                }
            }

            await batch.ExecuteAsync();

            batch = client.NewBatch();

            foreach (var teamMember in teamMemberInfo.Where(t => t.UserAction == UserAction.Add || t.UserAction == UserAction.Update))
            {
                foreach (var relatedActivity in jobApplication.JobApplicationActivities)
                {
                    var activityParticipant = relatedActivity.JobApplicationActivityParticipants.FirstOrDefault(p => p.JobOpeningParticipant?.RecId == teamMember.jobOpeningParticipant.RecId);
                    if (activityParticipant == null)
                    {
                        batch.Add(client.Create(GetJobApplicationActivityParticipantToCreate(teamMember.jobOpeningParticipant, relatedActivity, (int)teamMember.Ordinal.GetValueOrDefault())));
                    }
                    else
                    {
                        var newOrdinal = (int)teamMember.Ordinal.GetValueOrDefault();
                        if (newOrdinal != activityParticipant.Ordinal)
                        {
                            activityParticipant.Ordinal = (int)teamMember.Ordinal.GetValueOrDefault();
                            batch.Add(client.Update(activityParticipant.RecId.Value, activityParticipant, p => p.Ordinal));
                        }
                    }
                }
            }

            await batch.ExecuteAsync();
        }


        #region Private Methods
        private static IXrmHttpClientAction UpdateApplicationRecord(IXrmHttpClient client, JobApplication application, Expression<Func<JobApplication, object>> fields)
        {
            return client.Update(application, fields, UpsertBehavior.AllowUpdateIfEtagMatches);
        }

        private static JobApplicationActivityParticipant GetJobApplicationActivityParticipantToCreate(JobOpeningParticipant jobOpeningParticipant, JobApplicationActivity activity = null, int? ordinal = null)
        {
            Contract.CheckValue(jobOpeningParticipant, nameof(jobOpeningParticipant));

            return new JobApplicationActivityParticipant
            {
                JobOpeningParticipant = jobOpeningParticipant,
                JobApplicationActivity = activity,
                Ordinal = ordinal.GetValueOrDefault(),
            };
        }

        private async Task<JobApplication> GetRelatedJobApplicationActivitiesById(string jobApplicationActivityId)
        {
            Contract.CheckNonEmpty(jobApplicationActivityId, nameof(jobApplicationActivityId));

            this.Trace.TraceInformation($"Get job application activity by id");

            /* TODO: SRING - replace with getClient*/
            var client = await this.GetAdminClient();

            var activity = await client
                .Get<JobApplicationActivity>(
                    jaa => jaa.Autonumber == jobApplicationActivityId,
                    expand: f => new { f.JobOpeningStageActivity, f.JobApplication })
                .ExecuteAndGetAsync("HcmAttXrmGetRelJobAppAct1");

            var stageActivity = activity?.JobOpeningStageActivity;
            if (stageActivity?.JobOpeningStageId == null)
            {
                throw new Exception($"No activity found with id {jobApplicationActivityId}");
            }

            var jobApplication = activity.JobApplication;
            if (jobApplication == null)
            {
                throw new Exception($"No activity found with id  {activity.JobApplicationId.ToString()}");
            }

            var batch = client.NewBatch();
            jobApplication.JobOpening = new JobOpening { RecId = jobApplication.JobOpeningId };
            QueryJobOpeningParticipants(batch, jobApplication.JobOpening);
            QueryJobApplicationActivitesWithJobOpeningStageActivities(
                batch,
                jobApplication,
                jobOpeningStageActivityFilter: a => a.JobOpeningStageId == stageActivity.JobOpeningStageId && a.Ordinal == stageActivity.Ordinal);
            await batch.ExecuteAsync("HcmAttXrmGetRelJobAppAct2");

            batch = client.NewBatch();
            QueryJobApplicationActivityParticipants(batch, jobApplication.JobApplicationActivities);
            await batch.ExecuteAsync("HcmAttXrmGetRelJobAppAct3");

            foreach (var activityParticipant in jobApplication.JobApplicationActivities.SelectMany(a => a.JobApplicationActivityParticipants))
            {
                activityParticipant.JobOpeningParticipant =
                    jobApplication.JobOpening.JobOpeningParticipants.FirstOrDefault(p => p.RecId == activityParticipant.JobOpeningParticipantId);
            }

            return jobApplication;
        }

        private static void QueryJobApplicationActivitesWithJobOpeningStageActivities(
            IXrmHttpClientBatch batch,
            JobApplication jobApplication,
            Expression<Func<JobOpeningStageActivity, bool>> jobOpeningStageActivityFilter = null,
            Expression<Func<JobOpeningStage, bool>> jobOpeningStageFilter = null)
        {
            var fetch = new FetchXmlQuery<JobApplicationActivity>(FetchXmlFilter.Filter<JobApplicationActivity>(a => a.JobApplicationId == jobApplication.RecId), selectAllFields: true);
            var josaLinkEntity = fetch.AddInnerJoinManyToOne<JobOpeningStageActivity>(
                a => a.JobOpeningStageActivityId,
                jobOpeningStageActivityFilter,
                alias: "JOSA",
                selectAllFields: true);
            if (jobOpeningStageFilter != null)
            {
                josaLinkEntity.AddInnerJoinManyToOne<JobOpeningStage>(a => a.JobOpeningStageId, jobOpeningStageFilter);
            }

            batch.Add(
                batch.Client.GetAllWithFetchXml<JobApplicationActivity, JobApplicationActivity>(fetch),
                r =>
                {
                    if (r?.Result == null)
                    {
                        throw new InvalidOperationException("Could not query job applications activities!");
                    }

                    jobApplication.JobApplicationActivities = r.Result
                        .Select(activity =>
                        {
                            activity.JobOpeningStageActivity = activity.GetJoinedFetchXmlRecord<JobOpeningStageActivity>("JOSA");
                            return activity;
                        }).ToArray();
                });
        }
        #endregion
    }
}
