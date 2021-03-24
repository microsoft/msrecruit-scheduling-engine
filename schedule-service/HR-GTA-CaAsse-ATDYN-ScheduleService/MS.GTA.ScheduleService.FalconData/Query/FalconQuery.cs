//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ScheduleService.FalconData.Query
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using CommonLibrary.Common.Base.Configuration;
    using CommonLibrary.Common.Email.Contracts;
    using CommonLibrary.Common.TalentEntities.Common;
    using MS.GTA.ScheduleService.FalconData.ViewModelExtensions;
    using CommonLibrary.Common.Web.Contracts;
    using MS.GTA.Talent.FalconEntities.IV.Entity;
    using CommonLibrary.Common.Provisioning.Entities.FalconEntities.Attract;

    /// <summary>
    /// The falcon query.
    /// </summary>
    public class FalconQuery
    {
        /// <summary>
        /// The instance for <see cref="ILogger{FalconQuery}"/>
        /// </summary>
        private readonly ILogger<FalconQuery> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="FalconQuery" /> class
        /// </summary>
        /// <param name="falconQueryClient">The CDS query client used to get the CDS client for all the requests.</param>
        /// <param name="logger">The instance for <see cref="ILogger{FalconQuery}"/>.</param>
        public FalconQuery(
            IFalconQueryClient falconQueryClient,
            ILogger<FalconQuery> logger)
        {
            this.logger = logger;
            this.FalconQueryClient = falconQueryClient;

            this.ConfigurationManager = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<DocDBConfiguration>();
        }

        /// <summary>
        /// Gets or sets the CDS query client.
        /// </summary>
        internal IFalconQueryClient FalconQueryClient { get; set; }

        /// <summary>
        /// Gets or sets the db configuration
        /// </summary>
        internal DocDBConfiguration ConfigurationManager { get; set; }

        /// <summary>
        /// Get workers with oids
        /// </summary>
        /// <param name="ids">worker ids</param>
        /// <returns>workers</returns>
        public virtual async Task<List<Worker>> GetWorkers(List<string> ids)
        {
            if (ids != null && ids.Any())
            {
                var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);

                var data = await client.Get<Worker>(w => ids.Contains(w.OfficeGraphIdentifier));
                if (data != null)
                {
                    return data.ToList();
                }
            }

            return null;
        }

        /// <summary>
        /// Get workers with oids
        /// </summary>
        /// <param name="id">worker ids</param>
        /// <returns>workers</returns>
        public virtual async Task<Worker> GetWorker(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);
                return await client.GetFirstOrDefault<Worker>(w => w.OfficeGraphIdentifier.Equals(id));
            }

            return null;
        }

        /// <summary>
        /// Get Email template
        /// </summary>
        /// <param name="templateId">email template id</param>
        /// <returns>Email Template</returns>
        public async Task<EmailTemplate> GetTemplate(string templateId)
        {
            if (string.IsNullOrEmpty(templateId))
            {
                this.logger.LogError("GetTemplate: invalid template id");
                return null;
            }

            var client = await this.FalconQueryClient.GetFalconClient();
            if (client == null)
            {
                client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);
            }

            var emailTemplate = await client.Get<FalconEmailTemplate>(templateId);

            return emailTemplate?.ToContract();
        }

        /// <summary>
        /// Get Role Assignment
        /// </summary>
        /// <param name="userOid">user identifier</param>
        /// <returns>Roles</returns>
        public async Task<IEnumerable<IVApplicationRole>> GetRoleAssignment(string userOid)
        {
            IList<IVApplicationRole> roles = new List<IVApplicationRole>();
            if (string.IsNullOrWhiteSpace(userOid))
            {
                this.logger.LogError("GetRoleAssignment: invalid user oid");
                return roles;
            }

            var client = await this.FalconQueryClient.GetFalconClient();
            if (client == null)
            {
                client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.IVScheduleContainerId);
            }

            var jobIVUser = await client.GetFirstOrDefault<JobIVUser>(u => u.Person.ObjectId == userOid);
            if (jobIVUser != null)
            {
                roles = jobIVUser.Roles;
            }

            return roles;
        }

        /// <summary>
        /// Get Wob Delegates with User Office Graph Identifiers
        /// </summary>
        /// <param name="userId">User <see cref="Delegation"/> Office Graph Identifier</param>
        /// <returns>A list of <see cref="Delegation"/></returns>
        public virtual async Task<List<Delegation>> GetWobUsersDelegation(string userId)
        {
            List<Delegation> delegates = new List<Delegation>();
            if (!string.IsNullOrWhiteSpace(userId))
            {
                var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);
                var delegateResult = await client.Get<Delegation>(w => w.From.OfficeGraphIdentifier.Equals(userId)
                && w.DelegationStatus == TalentEntities.Enum.DelegationStatus.Active
                && w.RequestStatus == TalentEntities.Enum.RequestStatus.Active)
                    .ConfigureAwait(false);
                if (delegateResult != null)
                {
                    delegates = delegateResult.ToList();
                }
            }
            else
            {
                this.logger.LogError("The User Office Graph Identifier cannot be empty. No results returned");
            }

            return delegates;
        }

        /// <summary>
        /// Gets delegation for a  user id.
        /// </summary>
        /// <param name="userIds">user oid</param>
        /// <returns>List of active Delegations</returns>
        public async Task<List<Delegation>> GetWobUsersDelegationAsync(List<string> userIds)
        {
            List<Delegation> delegates = new List<Delegation>();
            if (userIds.Any())
            {
                var client = await this.FalconQueryClient.GetFalconClient(this.ConfigurationManager.DatabaseId, this.ConfigurationManager.CommonContainerId);
                var delegateResult = await client.Get<Delegation>(w => userIds.Contains(w.From.OfficeGraphIdentifier)
                && w.DelegationStatus == TalentEntities.Enum.DelegationStatus.Active)
                    .ConfigureAwait(false);
                if (delegateResult != null)
                {
                    delegates = delegateResult.ToList();
                }
            }
            else
            {
                this.logger.LogError("The User Office Graph Identifier cannot be empty. No results returned");
            }

            return delegates;
        }
    }
}
