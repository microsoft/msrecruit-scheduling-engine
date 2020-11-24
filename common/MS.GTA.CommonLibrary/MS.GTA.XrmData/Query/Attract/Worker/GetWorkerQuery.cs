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
    using Microsoft.Extensions.Logging;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Attract;
    using MS.GTA.Common.Provisioning.Entities.XrmEntities.Common;
    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.Common.XrmHttp;
    using MS.GTA.CommonDataService.Common.Internal;

    /// <summary>
    /// The worker query.
    /// </summary>
    public partial class XrmQuery : IQuery
    {
        /// <summary>
        /// Find workers.
        /// </summary>
        /// <param name="batch">The Xrm client batch.</param>
        /// <param name="workers">Workers to upsert.</param>
        public static void FindWorkers(IXrmHttpClientBatch batch, IList<Worker> workers)
        {
            FindWorkers(batch, workers, out var workersNotFound);
        }

        /// <summary>
        /// Find workers.
        /// </summary>
        /// <param name="batch">The Xrm client batch.</param>
        /// <param name="workers">Workers to upsert.</param>
        /// <param name="workersNotFound">Workers not found.</param>
        public static void FindWorkers(IXrmHttpClientBatch batch, IList<Worker> workers, out List<Worker> workersNotFound)
        {
            Contract.CheckValue(batch, nameof(batch));
            Contract.CheckValue(workers, nameof(workers));

            var workersNotFoundByAnotherName = workersNotFound = new List<Worker>();

            foreach (var worker in workers)
            {
                Contract.CheckNonEmpty(worker?.OfficeGraphIdentifier, nameof(worker.OfficeGraphIdentifier));
            }

            // Find existing workers.
            var workerOIDs = new HashSet<string>(workers.Where(w => !string.IsNullOrEmpty(w?.OfficeGraphIdentifier)).Select(w => w.OfficeGraphIdentifier));

            batch.Client.Logger.LogInformation($"FindWorkers: looking up {workerOIDs.Count} OIDs for {workers.Count} workers");
            if (workerOIDs.Any())
            {
                batch.Add(
                    batch.Client.GetAll<Worker>(w => workerOIDs.Contains(w.OfficeGraphIdentifier))
                        .Expand(w => w.SystemUser, u => new { u.SystemUserId, u.AzureActiveDirectoryObjectId, u.PrimaryEmail, u.FullName, u.FirstName, u.MiddleName, u.LastName, u.Title, u.IsDisabled, u.IsLicensed }),
                    deserializedResponseCallback: r =>
                    {
                        batch.Client.Logger.LogInformation($"FindWorkers: found {r.Result.Count} existing workers");
                        foreach (var worker in workers)
                        {
                            var workerFound = r.Result.FirstOrDefault(w => w.OfficeGraphIdentifier == worker.OfficeGraphIdentifier && w.SystemUserId != null)
                                           ?? r.Result.FirstOrDefault(w => w.OfficeGraphIdentifier == worker.OfficeGraphIdentifier);
                            if (workerFound == null)
                            {
                                batch.Client.Logger.LogInformation($"FindWorkers: did not find existing worker by OID");
                                workersNotFoundByAnotherName.Add(worker);
                            }
                            else
                            {
                                batch.Client.Logger.LogInformation($"FindWorkers: found existing worker by OID");
                                foreach (var property in typeof(Worker).GetProperties())
                                {
                                    var value = property.GetValue(workerFound);
                                    if (value != null)
                                    {
                                        property.SetValue(worker, value);
                                    }
                                }
                            }
                        }
                    });
            }
        }

        public async Task<Worker> GetWorkerByExternalId(TalentSource externalSource, string externalId)
        {
            if (string.IsNullOrEmpty(externalId))
            {
                this.Trace.TraceInformation("Worker external id is null or empty");
                return null;
            }

            // Get the worker record association with external id
            var client = await this.GetClient();

            throw new NotImplementedException("GetWorkerByExternalId: ExternalReference, Source");
            ////var workers = await client
            ////    .GetAll<Worker>(w => w.ExternalReference == externalId && w.Source == externalSource)
            ////    .ExecuteAndGetAsync("HcmAttXrmGetWorkerByExternalId");
            ////return workers?.FirstOrDefault();
        }

        public async Task<IList<Worker>> GetWorkerListByUserObjectId(string userObjectId)
        {
            Contract.CheckNonEmpty(userObjectId, nameof(userObjectId));

            var client = await this.GetClient();
            var worker = await client.GetAll<Worker>(w => w.OfficeGraphIdentifier == userObjectId).ExecuteAndGetAsync("HcmAttXrmGetWorker");

            return worker.Result;
        }

        public async Task ClearWorkerPrivateData(string oid)
        {
            if (string.IsNullOrWhiteSpace(oid))
            {
                return;
            }

            var client = await this.GetClient();
            var workers = (await client.GetAll<Worker>(worker => worker.OfficeGraphIdentifier == oid).ExecuteAndGetAsync("HcmXrmClearWorker"))?.Result;

            if (workers == null)
            {
                return;
            }

            var clearBatch = client.NewBatch();
            foreach (var currentWorker in workers)
            {
                if (currentWorker.RecId == null)
                {
                    continue;
                }

                currentWorker.EmailPrimary = string.Empty;
                currentWorker.FullName = string.Empty;
                currentWorker.PhonePrimary = string.Empty;
                currentWorker.LinkedInIdentity = string.Empty;

                clearBatch.Add(
                    client.Update(
                        currentWorker.RecId.Value,
                        currentWorker,
                        w => new
                        {
                            w.EmailPrimary,
                            w.FullName,
                            w.PhonePrimary,
                            w.LinkedInIdentity,
                        }));
            }

            await clearBatch.ExecuteAsync("HcmAttrXrmClearWorkerBatch");
        }
    }
}
