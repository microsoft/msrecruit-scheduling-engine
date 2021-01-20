// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using MS.GTA.Common.Base.Helper;
    using MS.GTA.Common.XrmHttp.Model.Metadata;
    using MS.GTA.ServicePlatform.Context;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;

    public static class XrmHttpClientExtensions
    {

        public static async Task<HttpResponseMessage> SendAsyncWithRetry(this IXrmHttpClient client, Func<HttpRequestMessage> getHttpRequestMessage, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            var retryCount = 0;
            while (true)
            {
                using (var request = getHttpRequestMessage())
                {
                    var response = await client.SendAsync(request);
                    if (response.StatusCode == HttpStatusCode.InternalServerError && retryCount < 2)
                    {
                        var retryDelay = TimeSpan.FromSeconds((1 + new Random().NextDouble()) * (1 + retryCount * retryCount));
                        retryCount++;

                        var contentString = response.Content == null ? null : await response.Content.ReadAsStringAsync();
                        client.Logger.LogError($"XrmHttpClientExtensions.SendAsyncWithRetry: retrying (count={retryCount}) after {retryDelay} due to {response.StatusCode} ({response.ReasonPhrase}): {contentString}");
                        response.Dispose();
                        await Task.Delay(retryDelay);
                        continue;
                    }

                    return response;
                }
            }
        }
        
        public static async Task ExecuteAsync(this IXrmHttpClientAction action, string activityName)
        {
            await action.ExecuteAsync(action.Client.Logger, activityName);
        }

        public static async Task ExecuteAsync(this IXrmHttpClientAction action, ILogger logger, string activityName)
        {
            await logger.ExecuteAsync(activityName, () => action.ExecuteAsync());
        }

        public static async Task<T> ExecuteAndGetAsync<T>(this IXrmHttpClientQuery<T> query, string activityName)
        {
            return await query.ExecuteAndGetAsync<T>(query.Client.Logger, activityName);
        }

        public static async Task<T> ExecuteAndGetAsync<T>(this IXrmHttpClientQuery<T> query, ILogger logger, string activityName)
        {
            return await logger.ExecuteAsync(activityName, () => query.ExecuteAndGetAsync());
        }
        public static async Task ExecuteAsync(this IXrmHttpClientBatch batch, string activityName)
        {
            await batch.ExecuteAsync(batch.Client.Logger, activityName);
        }

        public static async Task ExecuteAsync(this IXrmHttpClientBatch batch, ILogger logger, string activityName)
        {
            await logger.ExecuteAsync(activityName, () => batch.ExecuteAsync());
        }

        public static void CreateAll<T>(this IXrmHttpClientBatch batch, IEnumerable<T> recordsToCreate, Action<T, Guid> callback)
            where T : ODataEntity
        {
            foreach (var record in recordsToCreate)
            {
                batch.Add(batch.Client.Create(record), r => callback(record, r));
            }
        }

        // TODO
        /*
        public static void DeleteAll<T>(this IXrmHttpClientBatch batch, IEnumerable<T> records)
            where T : ODataEntity
        {
            foreach (var record in records)
            {
                var recordId = ODataEntityContractInfo.GetKeyPropertyValue(record);
                if (recordId.HasValue)
                {
                    batch.Add(batch.Client.Delete<T>(recordId.Value));
                }
            }
        }

        public static void DeleteAll<T>(this IXrmHttpClientBatch batch, IEnumerable<Guid> recordIds)
            where T : ODataEntity
        {
            foreach (var recordId in recordIds)
            {
                batch.Add(batch.Client.Delete<T>(recordId));
            }
        }
        */
        public static async Task DeleteAllRecordsFromTheEnvironment<T>(this IXrmHttpClient xrmClient, int chunkSize = 50, Expression<Func<T, bool>> filter = null)
            where T : ODataEntity
        {
            var keyProperty = ODataEntityContractInfo.GetKeyProperty(typeof(T));
            while (true)
            {
                var response =
                    await xrmClient.GetAll<T>(filterExpression: ODataExpression.Filter(filter), selectFields: new[] { ODataField.Field(keyProperty) }, maxpagesize: 5000)
                        .ExecuteAndGetAsync("HcmXrmDeleteAllQuery");
                if (response?.Result?.Any() != true)
                {
                    break;
                }

                foreach (var chunk in response.Result.Chunk(chunkSize))
                {
                    var batch = xrmClient.NewBatch();
                    batch.ContinueOnError = true;
                    foreach (var record in chunk)
                    {
                        batch.Add(xrmClient.Delete<T>((keyProperty.GetValue(record) as Guid?).Value));
                    }
                    await batch.ExecuteAsync("HcmXrmDeleteAllDelete");
                }
            }
        }

        public static async Task<ODataResponseList<TResponse>> GetAllWithFetchXmlUsingSdk<TQuery, TResponse>(this IXrmHttpClient xrmClient, string activityName, FetchXmlQuery<TQuery> fetchXmlQuery)
        {
            return await xrmClient.Logger.ExecuteAsync(activityName, async () =>
            {
                var queryString = fetchXmlQuery.ToString();
                xrmClient.Logger.LogInformation($"XrmHttpClient.GetAllWithFetchXmlUsingSdk: Request info BaseAddress={xrmClient.BaseAddress} Entity={typeof(TQuery).Name} Response={typeof(TResponse).Name} QueryLength={queryString.Length}");

                var startTime = DateTime.UtcNow;
                using (var organizationService = await xrmClient.GetOrganizationService())
                {
                    var entityCollection = await organizationService.RetrieveMultipleAsync(new Microsoft.Xrm.Sdk.Query.FetchExpression(queryString));
                    xrmClient.Logger.LogInformation($"XrmHttpClient.GetAllWithFetchXmlUsingSdk: Completed request BaseAddress={xrmClient.BaseAddress} Entity={typeof(TQuery).Name} Response={typeof(TResponse).Name} - got {entityCollection.Entities.Count} {entityCollection.EntityName} rows (total={entityCollection.TotalRecordCount}, more={entityCollection.MoreRecords}) after {DateTime.UtcNow - startTime}");

                    return new ODataResponseList<TResponse>()
                    {
                        ODataNextLink = entityCollection.PagingCookie,
                        ODataCount = entityCollection.TotalRecordCount,
                        Result = entityCollection.Entities.Select(e => e.ToContractClass<TResponse>()).ToArray(),
                    };
                }
            });
        }

        /*

        public static async Task<IList<T>> GetAllMultipage<T>(this IXrmHttpClient xrmClient, string activityName, Expression<Func<T, object>> select, int maxpagesize = 5000)
            where T : ODataEntity
        {
            return await xrmClient.GetAll(filter: null, select: select, maxpagesize: maxpagesize).ExecuteAndGetMultipage(xrmClient, activityName, maxpagesize);
        }
        */
        public static async Task<IList<T>> ExecuteAndGetMultipage<T>(this IXrmHttpClientQuery<ODataResponseList<T>> query, string activityName, int maxpagesize = 5000)
            where T : ODataEntity
        {
            var list = new List<T>();
            await query.ExecuteAndGetMultipageWithCallback(activityName, r => list.AddRange(r), maxpagesize);
            return list;
        }

        public static async Task<IList<T>> ExecuteAndGetMultipage<T>(this IXrmHttpClientQuery<ODataResponseList<T>> query, IXrmHttpClient xrmClient, string activityName, int maxpagesize = 5000)
            where T : ODataEntity
        {
            return await query.ExecuteAndGetMultipage(activityName, maxpagesize);
        }

        public static async Task ExecuteAndGetMultipageWithCallback<T>(this IXrmHttpClientQuery<ODataResponseList<T>> query, string activityName, Action<IList<T>> action, int maxpagesize = 5000)
            where T : ODataEntity
        {
            var response = await query.ExecuteAndGetAsync(activityName);
            action(response.Result);
            while (!string.IsNullOrEmpty(response.ODataNextLink))
            {
                response = await query.Client.GetNextPage(response, maxpagesize: maxpagesize).ExecuteAndGetAsync(activityName);
                action(response.Result);
            }
        }

        public static async Task ExecuteAndGetMultipageWithCallback<T>(this IXrmHttpClientQuery<ODataResponseList<T>> query, IXrmHttpClient xrmClient, string activityName, Action<IList<T>> action, int maxpagesize = 5000)
            where T : ODataEntity
        {
            await query.ExecuteAndGetMultipageWithCallback(activityName, action, maxpagesize);
        }
        
        public static void AddWithFinishMultipage<T>(this IXrmHttpClientBatch batch, IXrmHttpClientQuery<ODataResponseList<T>> query, Action<IList<T>> deserializedResponseCallback)
            where T : ODataEntity
        {
            batch.Add(
                query,
                httpResponseCallback: async httpResponse =>
                {
                    var response = await query.HandleQueryResponse(httpResponse);
                    var list = response.Result.ToList();
                    while (!string.IsNullOrEmpty(response.ODataNextLink))
                    {
                        response = await query.Client.GetNextPage(response).ExecuteAndGetAsync($"HcmXrmFinishMultipage{typeof(T).Name}");
                        list.AddRange(response.Result);
                    }
                    deserializedResponseCallback(list);
                });
        }

        public static IXrmHttpClientAction UpsertByKeyField<T>(this IXrmHttpClient client, T entity)
            where T : ODataEntity
        {
            var id = ODataEntityContractInfo.GetKeyPropertyValue(entity);
            if (id == null)
            {
                throw new InvalidOperationException($"Key field for entity of type {typeof(T).Name} must have a value");
            }

            return client.UpsertByKeyFieldValue<T>(id.Value, entity);
        }

        public static IXrmHttpClientAction UpsertByKeyFieldValue<T>(this IXrmHttpClient client, Guid id, T entity)
            where T : ODataEntity
        {
            var requestUri = ODataPath<T>.FromId(id).ToUri(client.BaseAddress);
            var contentString = JsonConvert.SerializeObject(entity, XrmHttpClient.CreateJsonSerializerSettings);

            return new XrmHttpClientAction(client.Logger, client, new HttpMethod("PATCH"), requestUri, contentString);
        }

        public static IXrmHttpClientAction UpsertByKeyExpression<T>(this IXrmHttpClient client, Expression<Func<T, bool>> keyExpression, T entity)
            where T : ODataEntity
        {
            var requestUri = ODataPath<T>.FromKeyExpression(keyExpression).ToUri(client.BaseAddress);
            var contentString = JsonConvert.SerializeObject(entity, XrmHttpClient.CreateJsonSerializerSettings);

            return new XrmHttpClientAction(client.Logger, client, new HttpMethod("PATCH"), requestUri, contentString);
        }

        public static IXrmHttpClientAction SetAutonumberSeed<T>(this IXrmHttpClient xrmClient, Expression<Func<T, object>> autonumberField, uint autonumberSeed)
            where T : ODataEntity
        {
            var request = new
            {
                EntityName = ODataEntityContractInfo.GetEntitySingularName(typeof(T)),
                AttributeName = ODataField.Field(autonumberField).ToString(),
                Value = autonumberSeed,
            };
            xrmClient.Logger.LogInformation($"SetAutonumberSeed: setting autonumber seed for {request.EntityName} field {request.AttributeName} to {request.Value}");
            return xrmClient.Action<dynamic, dynamic>("SetAutoNumberSeed", request);
        }

        public static void AssociateAll<T, TOther>(this IXrmHttpClientBatch batch, T mainRecord, IList<TOther> otherRecords, Expression<Func<T, IEnumerable<TOther>>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity
        {
            foreach (var otherRecord in otherRecords)
            {
                batch.Add(batch.Client.Associate(mainRecord, otherRecord, relationship));
            }
        }
        

        public static void DissociateAll<T, TOther>(this IXrmHttpClientBatch batch, T mainRecord, IList<TOther> otherRecords, Expression<Func<T, IEnumerable<TOther>>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity
        {
            foreach (var otherRecord in otherRecords)
            {
                batch.Add(batch.Client.Dissociate(mainRecord, otherRecord, relationship));
            }
        }

        public static T GetJoinedFetchXmlRecord<T>(this ODataEntity entity, string alias)
            where T : new()
        {
            var jsonSerializer = JsonSerializer.Create(XrmHttpClient.DefaultJsonSerializerSettings);
            var contract = jsonSerializer.ContractResolver.ResolveContract(typeof(T)) as JsonObjectContract;

            var fieldsStartingWithAlias = entity
                ?.ODataUnmappedFields
                ?.Where(p => p.Key.StartsWith(alias + "."))
                .Select(p =>
                {
                    var propertyName = p.Key.Substring(alias.Length + 1);
                    var contractProperty = contract.Properties.GetClosestMatchProperty($"_{propertyName}_value");
                    return new JProperty(contractProperty?.PropertyName ?? propertyName, p.Value);
                })
                .ToArray();
            if (fieldsStartingWithAlias == null || !fieldsStartingWithAlias.Any())
            {
                return default(T);
            }

            return new JObject(fieldsStartingWithAlias).ToObject<T>(jsonSerializer);
        }

        public static void QueryExpandAllForList<TParent, TChild>(
            this IXrmHttpClientBatch batch,
            IList<TParent> parents,
            Func<TParent, Guid?> parentGetField,
            Action<TParent, IList<TChild>> parentAssignChildren,
            Expression<Func<TChild, Guid?>> childField,
            int listChunkSize = 20,
            Expression<Func<TChild, object>> select = null,
            Expression<Func<TChild, object>> expand = null)
            where TParent : ODataEntity
            where TChild : ODataEntity
        {
            if (parents == null)
            {
                return;
            }

            var parentGroups = parents.Where(p => p != null).GroupBy(parentGetField).ToArray();
            foreach (var parent in parentGroups.Where(g => g.Key == null).SelectMany(g => g))
            {
                parentAssignChildren(parent, new List<TChild>());
            }

            var childODataField = ODataField.Field(childField);
            var selectFields = ODataField.Fields(select);
            var expandFields = ODataField.Fields(expand);

            if (selectFields != null && !selectFields.Any(f => f.ToString() == childODataField.ToString()))
            {
                selectFields = selectFields.Concat(new[] { childODataField }).ToArray();
            }

            var getChildField = childField.Compile();
            foreach (var parentGroupChunk in parentGroups.Where(g => g.Key != null).Chunk(listChunkSize))
            {
                batch.AddWithFinishMultipage(
                    batch.Client.GetAll<TChild>(ODataExpression.EqAny(childODataField, parentGroupChunk.Select(c => c.Key)), selectFields: selectFields, expandFields: expandFields),
                    r =>
                    {
                        foreach (var parentGroup in parentGroupChunk)
                        {
                            var childList = r.Where(tta => getChildField(tta) == parentGroup.Key).ToArray();
                            foreach (var parent in parentGroup)
                            {
                                parentAssignChildren(parent, childList.ToList());
                            }
                        }
                    });
            }
        }
        public static void QueryExpandForList<TParent, TChild>(
            this IXrmHttpClientBatch batch,
            IList<TParent> parents,
            Func<TParent, Guid?> parentGetField,
            Action<TParent, TChild> parentAssignChild,
            Expression<Func<TChild, Guid?>> childField,
            int listChunkSize = 20,
            Expression<Func<TChild, object>> select = null,
            Expression<Func<TChild, object>> expand = null)
            where TParent : ODataEntity
            where TChild : ODataEntity
        {
            if (parents == null)
            {
                return;
            }

            var parentGroups = parents.Where(p => p != null).GroupBy(parentGetField).ToArray();
            foreach (var parent in parentGroups.Where(g => g.Key == null).SelectMany(g => g))
            {
                parentAssignChild(parent, null);
            }

            var childODataField = ODataField.Field(childField);
            var selectFields = ODataField.Fields(select);
            var expandFields = ODataField.Fields(expand);

            if (selectFields != null && !selectFields.Any(f => f.ToString() == childODataField.ToString()))
            {
                selectFields = selectFields.Concat(new[] { childODataField }).ToArray();
            }

            var getChildField = childField.Compile();
            foreach (var parentGroupChunk in parentGroups.Where(g => g.Key != null).Chunk(listChunkSize))
            {
                batch.AddWithFinishMultipage(
                    batch.Client.GetAll<TChild>(ODataExpression.EqAny(childODataField, parentGroupChunk.Select(c => c.Key)), selectFields: selectFields, expandFields: expandFields),
                    r =>
                    {
                        foreach (var parentGroup in parentGroupChunk)
                        {
                            var child = r.FirstOrDefault(tta => getChildField(tta) == parentGroup.Key);
                            foreach (var parent in parentGroup)
                            {
                                parentAssignChild(parent, child);
                            }
                        }
                    });
            }
        }
        
        // TODO
        /*
        public static async Task RecalculateAutonumberSeed<T>(this IXrmHttpClient xrmClient, Expression<Func<T, object>> autonumberField, string prefix = null, uint defaultAutonumberSeed = 1000)
            where T : ODataEntity
        {
            prefix = prefix ?? string.Empty;
            var autonumberFieldFunc = autonumberField.Compile();
            var currentSeed = defaultAutonumberSeed;

            await xrmClient
                .GetAll(filter: null, select: autonumberField, maxpagesize: 5000)
                .ExecuteAndGetMultipageWithCallback("XrmRecalculateAutonumberSeed", r =>
                {
                    foreach (var record in r)
                    {
                        var autonumberString = autonumberFieldFunc(record) as string;
                        if (autonumberString != null
                            && autonumberString.StartsWith(prefix)
                            && uint.TryParse(autonumberString.Substring(prefix.Length), out var autonumberValue)
                            && autonumberValue >= currentSeed)
                        {
                            currentSeed = autonumberValue + 1;
                        }
                    }
                });

            await xrmClient.SetAutonumberSeed(autonumberField, currentSeed).ExecuteAsync("XrmRecalculateAutonumberSeedSet");
        }

        public static Task<T> CreateAndReturnWithAutonumberSeedRecalculate<T>(this IXrmHttpClient xrmClient, string activityName, T newRecord, Expression<Func<T, object>> autonumberField, string prefix = null, uint defaultAutonumberSeed = 1000)
            where T : ODataEntity
        {
            return xrmClient.Logger.ExecuteAsync(activityName, async () =>
            {
                for (var attempt = 0; true; attempt++)
                {
                    try
                    {
                        return await xrmClient.CreateAndReturn(newRecord).ExecuteAndGetAsync();
                    }
                    catch (XrmHttpClientDuplicateAlternateKeyValuesException e)
                        when (attempt < 3)
                    {
                        xrmClient.Logger.LogError($"CreateAndReturnWithAutonumberSeedRecalculate: Got duplicate alternate key exception - recalculating autonumber seed for {typeof(T).Name} (slow!) - attempt {attempt}: {e}");
                        await xrmClient.RecalculateAutonumberSeed(autonumberField, prefix, defaultAutonumberSeed);
                    }
                }
            });
        }

        public static async Task<bool> DoesEntityExist<T>(this IXrmHttpClient xrmClient)
            where T : ODataEntity
        {
            var entitySingularName = ODataEntityContractInfo.GetEntitySingularName(typeof(T));
            EntityMetadata result = null;

            try
            {
                result = await xrmClient.Get<EntityMetadata>(f => f.LogicalName == entitySingularName, s => s.LogicalName).ExecuteAndGetAsync(xrmClient.Logger, "XrmDoesEntityExist");
            }
            catch (XrmHttpClientResponseException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            return result != null;
        }*/
    }
}
