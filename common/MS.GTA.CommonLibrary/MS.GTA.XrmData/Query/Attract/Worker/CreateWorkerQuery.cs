//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.XrmData.Query
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CommonDataService.Common.Internal;
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.Common.Utils;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.Common.XrmHttp.Model;

    /// <summary>
    /// The worker query.
    /// </summary>
    public partial class XrmQuery : IQuery
    {
        /// <summary>
        /// Create workers
        /// </summary>
        /// <param name="batch">The Xrm client batch.</param>
        /// <param name="workers">Workers to create.</param>
        public static void CreateWorkers(IXrmHttpClientBatch batch, IList<Worker> workers)
        {
            if (workers.Any())
            {
                foreach (var workersWithSameId in workers.GroupBy(w => w.OfficeGraphIdentifier).ToArray())
                {
                    var worker = workersWithSameId.First();

                    Contract.CheckNonWhitespace(worker.OfficeGraphIdentifier, nameof(worker.OfficeGraphIdentifier));
                    Contract.CheckNonWhitespace(worker.EmailPrimary, nameof(worker.EmailPrimary));
                    Contract.CheckNonWhitespace(worker.FullName, nameof(worker.FullName));

                    // Prevent trying to deep-create system users.
                    if (worker.SystemUser?.SystemUserId == null)
                    {
                        worker.SystemUser = null;
                    }

                    batch.Add(
                        batch.Client.Create(worker),
                        entityIdCallback: id =>
                        {
                            foreach (var workerToUpdate in workersWithSameId)
                            {
                                workerToUpdate.RecId = id;
                            }
                        });
                }
            }
        }

        /// <summary>
        /// Find or create workers
        /// </summary>
        /// <param name="client">The Xrm client.</param>
        /// <param name="workers">Workers to upsert.</param>
        public async Task FindOrCreateWorkers(IXrmHttpClient client, IList<Worker> workers)
        {
            Contract.CheckValue(client, nameof(client));
            Contract.CheckValue(workers, nameof(workers));

            // Find existing workers.
            var batch = client.NewBatch();
            FindWorkers(batch, workers, out var workersNotFound);
            FindSystemUsersToBeAssignedToWorkers(batch, workers);
            await batch.ExecuteAsync("HcmAttXrmFindOrCreateWorkersExisting");

            // Update workers to match system user.
            await UpdateWorkersWithSystemUserInfo(client, workers);

            // Create not found workers.
            batch = client.NewBatch();
            CreateWorkers(batch, workersNotFound);
            await batch.ExecuteAsync("HcmAttXrmFindOrCreateWorkersCreate");

            // Sanity check.
            foreach (var worker in workers)
            {
                Contract.Check(worker.RecId != null, "FindOrCreateWorkers: worker.RecId should be set");
            }
        }

        private static async Task UpdateWorkersWithSystemUserInfo(IXrmHttpClient client, IList<Worker> workers)
        {
            foreach (var worker in workers)
            {
                if (worker.RecId != null)
                {
                    if (worker.SystemUser?.RecId != null)
                    {
                        var updatedWorker = new Worker
                        {
                            SystemUser = worker.SystemUser,
                            EmailPrimary = worker.SystemUser.PrimaryEmail ?? worker.EmailPrimary,
                            FullName = worker.SystemUser.FullName ?? worker.FullName ?? $"{worker.SystemUser.FirstName} {worker.SystemUser.LastName}",
                            GivenName = worker.SystemUser.FirstName ?? worker.GivenName,
                            MiddleName = worker.SystemUser.MiddleName ?? worker.MiddleName,
                            Surname = worker.SystemUser.LastName ?? worker.Surname,
                            Profession = worker.SystemUser.Title ?? worker.Profession,
                        };
                        if (worker.SystemUserId == null
                            || worker.SystemUserId != updatedWorker.SystemUser.RecId
                            || worker.EmailPrimary != updatedWorker.EmailPrimary
                            || worker.FullName != updatedWorker.FullName
                            || worker.GivenName != updatedWorker.GivenName
                            || worker.MiddleName != updatedWorker.MiddleName
                            || worker.Surname != updatedWorker.Surname
                            || worker.Profession != updatedWorker.Profession)
                        {
                            // Not batched as this can be very slow (this could apply all permissions for a user).
                            await client
                                .Update(worker.RecId.Value, updatedWorker, w => new { w.SystemUser, w.EmailPrimary, w.FullName, w.GivenName, w.MiddleName, w.Surname, w.Profession })
                                .ExecuteAsync("HcmAttXrmFindOrCreateWorkersUpdate");
                        }
                    }
                    else if (worker.SystemUserId != null && worker.SystemUser == null)
                    {
                        // Remove invalid user link.
                        await client.Dissociate(worker, w => w.SystemUser).ExecuteAsync("HcmAttXrmFindOrCreateWorkersUnlink");
                    }
                }
            }
        }

        private static void FindSystemUsersToBeAssignedToWorkers(IXrmHttpClientBatch batch, IList<Worker> workers)
        {
            var workerOIDs = new HashSet<Guid?>(workers.Select(w => w.OfficeGraphIdentifier.ToNullableGuid()).Where(g => g.HasValue));

            batch.Client.Logger.LogInformation($"FindSystemUsersToBeAssignedToWorkers: looking up {workerOIDs.Count} OIDs for {workers.Count} workers");

            if (!workerOIDs.Any())
            {
                return;
            }

            var filter = FetchXmlFilter.Filter<SystemUser>(u =>
                u.IsDisabled == false
                && u.IsLicensed == true
                && u.IsSyncWithDirectory == true
                && workerOIDs.Contains(u.AzureActiveDirectoryObjectId));
            var query = new FetchXmlQuery<SystemUser>(filter, distinct: true)
                .AddSelectField(u => u.SystemUserId)
                .AddSelectField(u => u.AzureActiveDirectoryObjectId)
                .AddSelectField(u => u.PrimaryEmail)
                .AddSelectField(u => u.FullName)
                .AddSelectField(u => u.FirstName)
                .AddSelectField(u => u.MiddleName)
                .AddSelectField(u => u.LastName)
                .AddSelectField(u => u.IsDisabled)
                .AddSelectField(u => u.IsLicensed);
            AddSystemUserRootRoleIdJoin(query, XrmRoleIds.TalentUserRoleId);

            batch.Add(
                batch.Client.GetAllWithFetchXml<SystemUser, SystemUser>(query),
                deserializedResponseCallback: r =>
                {
                    foreach (var worker in workers)
                    {
                        var workerOid = worker.OfficeGraphIdentifier.ToNullableGuid();
                        if (workerOid.HasValue)
                        {
                            // Validate the currently linked system user has the same OID.
                            if (worker.SystemUser?.RecId != null
                                && worker.SystemUser.AzureActiveDirectoryObjectId != null
                                && worker.SystemUser.AzureActiveDirectoryObjectId != workerOid)
                            {
                                batch.Client.Logger.LogWarning($"FindSystemUsersToBeAssignedToWorkers: worker {worker.RecId} (AAD id: {worker.OfficeGraphIdentifier}) incorrectly linked to system user {worker.SystemUser.RecId} (AAD id: {worker.SystemUser.AzureActiveDirectoryObjectId}), unlinking the currently linked system user");
                                // Don't copy any data from this system user to the worker.
                                // Also, allows the next block to link to the correct user going forwards.
                                worker.SystemUser = null;
                            }

                            // Validate the currently linked system user is enabled.
                            if (worker.SystemUser?.RecId != null
                                && (worker.SystemUser.IsDisabled == true
                                    || worker.SystemUser.IsLicensed != true))
                            {
                                batch.Client.Logger.LogWarning($"FindSystemUsersToBeAssignedToWorkers: worker {worker.RecId} (AAD id: {worker.OfficeGraphIdentifier}) "
                                    + $"linked to disabled or unlicensed system user {worker.SystemUser.RecId} "
                                    + $"(AAD id: {worker.SystemUser.AzureActiveDirectoryObjectId}, disabled: {worker.SystemUser.IsDisabled}, licensed: {worker.SystemUser.IsLicensed}), "
                                    + $"unlinking the currently linked system user");
                                worker.SystemUser = null;
                            }

                            // Attempt linking the worker to a system user.
                            if (worker.SystemUser?.RecId == null)
                            {
                                batch.Client.Logger.LogInformation($"FindSystemUsersToBeAssignedToWorkers: attempting linking worker {worker.RecId} (AAD id: {worker.OfficeGraphIdentifier}) to system user");
                                worker.SystemUser = r.Result.FirstOrDefault(u => u.AzureActiveDirectoryObjectId == workerOid);
                                if (worker.SystemUser?.RecId != null)
                                {
                                    batch.Client.Logger.LogInformation($"FindSystemUsersToBeAssignedToWorkers: linking worker {worker.RecId} (AAD id: {worker.OfficeGraphIdentifier}) to system user {worker.SystemUser.RecId} (AAD id: {worker.SystemUser.AzureActiveDirectoryObjectId})");
                                }
                                else
                                {
                                    batch.Client.Logger.LogInformation($"FindSystemUsersToBeAssignedToWorkers: could not link worker {worker.RecId} (AAD id: {worker.OfficeGraphIdentifier}) to system user");
                                }
                            }
                        }
                    }
                });
        }

        private void FindWorkerSystemUserForCurrentUser(IXrmHttpClientBatch batch, List<Worker> workersNotFound)
        {
            if (!string.IsNullOrEmpty(this.userPrincipal?.ObjectId) && Guid.TryParse(this.userPrincipal.ObjectId, out var objectId))
            {
                batch.Add(
                    batch.Client.GetAll<SystemUser>(u => u.AzureActiveDirectoryObjectId == objectId),
                    deserializedResponseCallback: r =>
                    {
                        foreach (var worker in workersNotFound)
                        {
                            worker.SystemUser = r.Result.FirstOrDefault(u => u.AzureActiveDirectoryObjectId?.ToString() == worker.OfficeGraphIdentifier);
                        }
                    });
            }
        }

        private static void AddSystemUserRootRoleIdJoin(FetchXmlQuery<SystemUser> query, params Guid[] roles)
        {
            query
                .AddInnerJoin<SystemUserRole>(u => u.SystemUserId, sur => sur.SystemUserId)
                .AddInnerJoin<Role>(sur => sur.RoleId, r => r.RecId, r => roles.Contains(r.ParentRootRoleId.Value));
        }
    }
}
