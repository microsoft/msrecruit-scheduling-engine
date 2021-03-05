//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.XrmHttp
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Base.Utilities;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;
    using Common.XrmHttp.RelevanceSearch;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class XrmHttpClient : IXrmHttpClient
    {
        /// <summary>
        /// The default page size.
        /// </summary>
        /// <remarks>Maximum page size is 5000.</remarks>
        public const int DefaultPageSize = 5000;

        private const int TotalCookies = 300;
        private const int CookiesPerDomain = 5;
        private const int MaxCookieSize = 4096;

        private const string RelevanceSearchApiVersion = "9.0";
        private const string RelevanceSearchUri = "/api/search/indexes/default/entities";
        private const string RelevanceSearchDefaultSkip = "0";
        private const string RelevanceSearchDefaultTake = "50";

        public static readonly JsonSerializerSettings DefaultJsonSerializerSettings = new JsonSerializerSettings
        {
            Converters = { new ODataEntityConverter() },
        };

        public static readonly JsonSerializerSettings CreateJsonSerializerSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            DefaultValueHandling = DefaultValueHandling.Ignore,
            Converters = { new ODataEntityConverter() },
        };

        private static HttpClient StaticHttpClient = new HttpClient(new HttpClientHandler
        {
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
            CookieContainer = new CookieContainer(TotalCookies, CookiesPerDomain, MaxCookieSize),
        })
        {
            Timeout = TimeSpan.FromSeconds(140),
        };

        private readonly Func<Task<Tuple<string, DateTime?>>> getToken;
        private readonly Func<Task<string>> getRootActivityId;
        private readonly Func<Task<string>> getActivityVector;
        private string bearerToken;
        private DateTime? tokenExpiration;

        public XrmHttpClient(ILogger logger, Uri baseAddress, Func<Task<Tuple<string, DateTime?>>> getToken, Func<Task<string>> getRootActivityId, Func<Task<string>> getActivityVector, bool tracePotentialPII = false)
        {
            this.BaseAddress = new Uri(baseAddress.ToString().TrimEnd('/') + "/");
            this.Logger = logger;
            this.getToken = getToken;
            this.getRootActivityId = getRootActivityId;
            this.getActivityVector = getActivityVector;
            this.ShouldTracePotentialPII = tracePotentialPII;

            logger.LogDebug($"XrmHttpClient: Constructed new instance with base address {baseAddress}");
        }

        public Uri BaseAddress { get; }

        public ILogger Logger { get; }

        /// <summary>
        /// The AAD object id for the user to impersonate.
        /// </summary>
        public Guid? ImpersonatedUserObjectId { get; set; }

        /// <summary>
        /// The primary key value from the systemuser entity for the user to impersonate.
        /// </summary>
        public Guid? ImpersonatedUser { get; set; }

        public bool ShouldTracePotentialPII { get; }
        
        public async Task<IXrmHttpClient> RefreshToken()
        {
            (var token, var expiration) = await this.getToken();
            this.bearerToken = token;
            this.tokenExpiration = expiration;
            this.Logger.LogDebug($"XrmHttpClient: Refreshed token (expiration={this.tokenExpiration})");
            return this;
        }

        /// <inheritdoc/>
        public IXrmHttpClientBatch NewBatch()
        {
            return new XrmHttpClientBatch(this);
        }

        /// <inheritdoc/>
        public IXrmHttpClientBatch NewAutoflushingBatch(int maximumActionsPerBatch = 50)
        {
            return new XrmHttpClientAutoflushingBatch(this, maximumActionsPerBatch);
        }

        #region Get

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> Get<T>(Guid id, IEnumerable<ODataField> selectFields, IEnumerable<ODataField> expandFields)
            where T : ODataEntity
        {
            return this.GetAtPath(ODataPath<T>.FromId(id), selectFields: selectFields, expandFields: expandFields);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> Get<T>(Guid id, Expression<Func<T, object>> select = null, Expression<Func<T, object>> expand = null)
            where T : ODataEntity
        {
            return this.GetAtPath(ODataPath<T>.FromId(id), select: select, expand: expand);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> Get<T>(IEnumerable<Tuple<ODataField, object>> keyValues, IEnumerable<ODataField> selectFields, IEnumerable<ODataField> expandFields)
            where T : ODataEntity
        {
            return this.GetAtPath(ODataPath<T>.FromKeyFieldValues(keyValues), selectFields: selectFields, expandFields: expandFields);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> Get<T>(Expression<Func<T, bool>> keyExpression, Expression<Func<T, object>> select = null, Expression<Func<T, object>> expand = null)
            where T : ODataEntity
        {
            return this.GetAtPath(ODataPath<T>.FromKeyExpression(keyExpression), select: select, expand: expand);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> GetAtPath<T>(ODataPath<T> path, IEnumerable<ODataField> selectFields, IEnumerable<ODataField> expandFields)
            where T : ODataEntity
        {
            var requestUri = path.ToUri(this.BaseAddress);
            requestUri = AddFieldListQueryString("$select", selectFields, requestUri);
            requestUri = AddFieldListQueryString("$expand", expandFields, requestUri);

            return new XrmHttpClientQuery<T>(this.Logger, this, HttpMethod.Get, requestUri, is404Ok: true);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> GetAtPath<T>(ODataPath<T> path, Expression<Func<T, object>> select = null, Expression<Func<T, object>> expand = null)
            where T : ODataEntity
        {
            return this.GetAtPath(path: path, selectFields: ODataField.Fields(select), expandFields: ODataField.Fields(expand));
        }

        #endregion

        #region Get all

        /// <inheritdoc/>
        public IXrmHttpClientQuery<ODataResponseList<T>> GetAll<T>(
            ODataExpression filterExpression = default(ODataExpression),
            IEnumerable<ODataField> selectFields = null,
            IEnumerable<ODataField> expandFields = null,
            IEnumerable<Tuple<string, bool>> orderBy = null,
            int maxpagesize = DefaultPageSize,
            bool count = false,
            int top = 0)
            where T : ODataEntity
        {
            return this.GetAllAtPath(
                ODataPath<T>.FromEntity(),
                filterExpression: filterExpression,
                selectFields: selectFields,
                expandFields: expandFields,
                maxpagesize: maxpagesize,
                count: count,
                top: top);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<ODataResponseList<T>> GetAll<T>(
            Expression<Func<T, bool>> filter,
            Expression<Func<T, object>> select = null,
            Expression<Func<T, object>> expand = null,
            int maxpagesize = DefaultPageSize,
            bool count = false,
            int top = 0)
            where T : ODataEntity
        {
            return this.GetAllAtPath(
                ODataPath<T>.FromEntity(),
                filter: filter,
                select: select,
                expand: expand,
                maxpagesize: maxpagesize,
                count: count,
                top: top);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<ODataResponseList<T>> GetAllAtPath<T>(
            ODataPath<IEnumerable<T>> path,
            ODataExpression filterExpression = default(ODataExpression),
            IEnumerable<ODataField> selectFields = null,
            IEnumerable<ODataField> expandFields = null,
            IEnumerable<Tuple<string, bool>> orderBy = null,
            int maxpagesize = DefaultPageSize,
            bool count = false,
            int top = 0)
            where T : ODataEntity
        {
            var requestUri = path.ToUri(this.BaseAddress);
            requestUri = AddFilterQueryString(filterExpression, requestUri);
            requestUri = AddFieldListQueryString("$select", selectFields, requestUri);
            requestUri = AddFieldListQueryString("$expand", expandFields, requestUri);
            requestUri = AddOrderByQueryString(orderBy, requestUri);
            requestUri = AddLimitQueryString("$top", top, requestUri);
            requestUri = count ? QueryHelpers.AddQueryString(requestUri, "$count", "true") : requestUri;

            return new XrmHttpClientQuery<ODataResponseList<T>>(
                this.Logger,
                this,
                HttpMethod.Get,
                requestUri,
                headers:
                maxpagesize >= 0
                    ? new[] { Tuple.Create("Prefer", $"odata.maxpagesize={maxpagesize}") }
                    : new Tuple<string, string>[] { });
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<ODataResponseList<T>> GetAllAtPath<T>(
            ODataPath<IEnumerable<T>> path,
            Expression<Func<T, bool>> filter,
            Expression<Func<T, object>> select = null,
            Expression<Func<T, object>> expand = null,
            int maxpagesize = DefaultPageSize,
            bool count = false,
            int top = 0)
            where T : ODataEntity
        {
            return this.GetAllAtPath(
                path,
                filterExpression: ODataExpression.Filter(filter),
                selectFields: ODataField.Fields(select),
                expandFields: ODataField.Fields(expand),
                maxpagesize: maxpagesize,
                count: count,
                top: top);
        }

        #endregion
        #region Get all with Xml filter

        /// <inheritdoc/>
        public IXrmHttpClientQuery<ODataResponseList<TResponse>> GetAllWithFetchXml<TQuery, TResponse>(FetchXmlQuery<TQuery> fetchXmlQuery)
        {
            return this.GetAllWithFetchXml<TQuery, TResponse>(fetchXmlQuery.ToString());
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<ODataResponseList<TResponse>> GetAllWithFetchXml<TQuery, TResponse>(string fetchXml)
        {
            var requestUri = ODataPath<TQuery>.FromEntity().ToUri(this.BaseAddress);
            requestUri = QueryHelpers.AddQueryString(requestUri, "fetchXml", fetchXml);

            return new XrmHttpClientQuery<ODataResponseList<TResponse>>(this.Logger, this, HttpMethod.Get, requestUri);
        }

        #endregion

        #region Get next page

        /// <inheritdoc/>
        public IXrmHttpClientQuery<ODataResponseList<T>> GetNextPage<T>(
                ODataResponseList<T> previousPage,
                int maxpagesize = DefaultPageSize)
            where T : ODataEntity
        {
            return new XrmHttpClientQuery<ODataResponseList<T>>(
                this.Logger,
                this,
                HttpMethod.Get,
                new Uri(previousPage.ODataNextLink).PathAndQuery,
                headers:
                    maxpagesize >= 0
                    ? new[] { Tuple.Create("Prefer", $"odata.maxpagesize={maxpagesize}") }
                    : new Tuple<string, string>[] { });
        }

        #endregion

        #region Create

        /// <inheritdoc/>
        public IXrmHttpClientAction Create<T>(T entity)
            where T : ODataEntity
        {
            return this.CreateAtPath(ODataPath<T>.FromEntity(), entity);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction CreateAtPath<T>(ODataPath<IEnumerable<T>> path, T entity)
            where T : ODataEntity
        {
            var requestUri = path.ToUri(this.BaseAddress);

            var contentString = JsonConvert.SerializeObject(entity, CreateJsonSerializerSettings);

            return new XrmHttpClientAction(this.Logger, this, HttpMethod.Post, requestUri, contentString);
        }

        #endregion

        #region CreateAndReturn

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> CreateAndReturn<T>(T entity, IEnumerable<ODataField> selectFields = null)
            where T : ODataEntity
        {
            return this.CreateAndReturnAtPath(ODataPath<T>.FromEntity(), entity, selectFields);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> CreateAndReturn<T>(T entity, Expression<Func<T, object>> select)
            where T : ODataEntity
        {
            return this.CreateAndReturnAtPath(ODataPath<T>.FromEntity(), entity, select);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> CreateAndReturnAtPath<T>(ODataPath<IEnumerable<T>> path, T entity, IEnumerable<ODataField> selectFields = null)
            where T : ODataEntity
        {
            var requestUri = path.ToUri(this.BaseAddress);
            requestUri = AddFieldListQueryString("$select", selectFields, requestUri);

            var contentString = JsonConvert.SerializeObject(entity, CreateJsonSerializerSettings);

            return new XrmHttpClientQuery<T>(this.Logger, this, HttpMethod.Post, requestUri, contentString, false, Tuple.Create("Prefer", "return=representation"));
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> CreateAndReturnAtPath<T>(ODataPath<IEnumerable<T>> path, T entity, Expression<Func<T, object>> select)
            where T : ODataEntity
        {
            return this.CreateAndReturnAtPath(path, entity, selectFields: ODataField.Fields(select));
        }

        #endregion

        #region Update

        /// <inheritdoc/>
        public IXrmHttpClientAction Update<T>(T entity, IEnumerable<ODataField> fields, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            var id = ODataEntityContractInfo.GetKeyPropertyValue(entity);
            return this.UpdateAtPath(ODataPath<T>.FromId(id.Value), entity, fields, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction Update<T>(T entity, Expression<Func<T, object>> fieldsExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            var id = ODataEntityContractInfo.GetKeyPropertyValue(entity);
            return this.UpdateAtPath(ODataPath<T>.FromId(id.Value), entity, fieldsExpression, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction Update<T>(Guid id, T entity, IEnumerable<ODataField> fields, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAtPath(ODataPath<T>.FromId(id), entity, fields, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction Update<T>(Guid id, T entity, Expression<Func<T, object>> fieldsExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAtPath(ODataPath<T>.FromId(id), entity, fieldsExpression, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction Update<T>(Guid id, Expression<Func<T>> entityCreateExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAtPath(ODataPath<T>.FromId(id), entityCreateExpression, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction Update<T>(Expression<Func<T, bool>> keyExpression, T entity, Expression<Func<T, object>> fieldsExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAtPath(ODataPath<T>.FromKeyExpression(keyExpression), entity, fieldsExpression, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction Update<T>(Expression<Func<T, bool>> keyExpression, Expression<Func<T>> entityCreateExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAtPath(ODataPath<T>.FromKeyExpression(keyExpression), entityCreateExpression, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction UpdateAtPath<T>(ODataPath<T> path, T entity, IEnumerable<ODataField> fields, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            var requestUri = path.ToUri(this.BaseAddress);

            var contentString = GetUpdateRequestContent(entity, fields);

            List<Tuple<string, string>> headers = new List<Tuple<string, string>>();
            switch (upsertBehavior)
            {
                case UpsertBehavior.AllowCreate:
                    headers.Add(Tuple.Create("If-None-Match", "*"));
                    break;
                case UpsertBehavior.AllowUpdate:
                    headers.Add(Tuple.Create("If-Match", "*"));
                    break;
                case UpsertBehavior.AllowUpdateIfEtagMatches:
                    headers.Add(Tuple.Create("If-Match", entity.ODataEtag));
                    break;
                case UpsertBehavior.AllowCreateOrUpdate:
                    break;
                default:
                    throw new ArgumentException($"Unknown upsertBehavior {upsertBehavior}");
            }

            return new XrmHttpClientAction(this.Logger, this, new HttpMethod("PATCH"), requestUri, contentString, is404Ok: false, headers: headers.ToArray());
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction UpdateAtPath<T>(ODataPath<T> path, T entity, Expression<Func<T, object>> fieldsExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAtPath(path, entity, fields: ODataField.Fields(fieldsExpression), upsertBehavior: upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction UpdateAtPath<T>(ODataPath<T> path, Expression<Func<T>> entityCreateExpression, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            var func = entityCreateExpression.Compile();
            return this.UpdateAtPath(path, entity: func(), fields: ODataField.InitializationFields(entityCreateExpression), upsertBehavior: upsertBehavior);
        }

        #endregion

        #region UpdateAndReturn

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> UpdateAndReturn<T>(T entity, IEnumerable<ODataField> fields, IEnumerable<ODataField> selectFields = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            var id = ODataEntityContractInfo.GetKeyPropertyValue(entity);
            return this.UpdateAndReturnAtPath(ODataPath<T>.FromId(id.Value), entity, fields, selectFields, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> UpdateAndReturn<T>(T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            var id = ODataEntityContractInfo.GetKeyPropertyValue(entity);
            return this.UpdateAndReturnAtPath(ODataPath<T>.FromId(id.Value), entity, fieldsExpression, select, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> UpdateAndReturn<T>(Guid id, T entity, IEnumerable<ODataField> fields, IEnumerable<ODataField> selectFields = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAndReturnAtPath(ODataPath<T>.FromId(id), entity, fields, selectFields, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> UpdateAndReturn<T>(Guid id, T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAndReturnAtPath(ODataPath<T>.FromId(id), entity, fieldsExpression, select, upsertBehavior);
        }

        public IXrmHttpClientQuery<T> UpdateAndReturnWithCustomFields<T>(Guid id, T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAndReturnAtPathWithCustomFields(ODataPath<T>.FromId(id), entity, fieldsExpression, select, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> UpdateAndReturn<T>(Guid id, Expression<Func<T>> entityCreateExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAndReturnAtPath(ODataPath<T>.FromId(id), entityCreateExpression, select, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> UpdateAndReturn<T>(Expression<Func<T, bool>> keyExpression, T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAndReturnAtPath(ODataPath<T>.FromKeyExpression(keyExpression), entity, fieldsExpression, select, upsertBehavior);
        }

        public IXrmHttpClientQuery<T> UpdateAndReturnWithCustomFields<T>(Expression<Func<T, bool>> keyExpression, T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAndReturnAtPathWithCustomFields(ODataPath<T>.FromKeyExpression(keyExpression), entity, fieldsExpression, select, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> UpdateAndReturn<T>(Expression<Func<T, bool>> keyExpression, Expression<Func<T>> entityCreateExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAndReturnAtPath(ODataPath<T>.FromKeyExpression(keyExpression), entityCreateExpression, select, upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> UpdateAndReturnAtPath<T>(ODataPath<T> path, T entity, IEnumerable<ODataField> fields, IEnumerable<ODataField> selectFields = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            var requestUri = path.ToUri(this.BaseAddress);
            requestUri = AddFieldListQueryString("$select", selectFields, requestUri);

            var contentString = GetUpdateRequestContent(entity, fields);

            List<Tuple<string, string>> headers = new List<Tuple<string, string>>();
            switch (upsertBehavior)
            {
                case UpsertBehavior.AllowCreate:
                    headers.Add(Tuple.Create("If-None-Match", "*"));
                    break;
                case UpsertBehavior.AllowUpdate:
                    headers.Add(Tuple.Create("If-Match", "*"));
                    break;
                case UpsertBehavior.AllowUpdateIfEtagMatches:
                    headers.Add(Tuple.Create("If-Match", entity.ODataEtag));
                    break;
                case UpsertBehavior.AllowCreateOrUpdate:
                    break;
                default:
                    throw new ArgumentException($"Unknown upsertBehavior {upsertBehavior}");
            }

            headers.Add(Tuple.Create("Prefer", "return=representation"));

            return new XrmHttpClientQuery<T>(this.Logger, this, new HttpMethod("PATCH"), requestUri, contentString, is404Ok: false, headers: headers.ToArray());
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> UpdateAndReturnAtPath<T>(ODataPath<T> path, T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            return this.UpdateAndReturnAtPath(path, entity, fields: ODataField.Fields(fieldsExpression), selectFields: ODataField.Fields(select), upsertBehavior: upsertBehavior);
        }

        public IXrmHttpClientQuery<T> UpdateAndReturnAtPathWithCustomFields<T>(ODataPath<T> path, T entity, Expression<Func<T, object>> fieldsExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            var fields = ODataField.Fields(fieldsExpression).ToList();
            entity.ODataUnmappedFields?.ForEach(each =>
            {
                var odataField = ODataField.FieldByName(each.Key);
                fields.Add(odataField);
            });

            return this.UpdateAndReturnAtPath(path, entity, fields: fields, selectFields: ODataField.Fields(select), upsertBehavior: upsertBehavior);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<T> UpdateAndReturnAtPath<T>(ODataPath<T> path, Expression<Func<T>> entityCreateExpression, Expression<Func<T, object>> select = null, UpsertBehavior upsertBehavior = UpsertBehavior.AllowUpdate)
            where T : ODataEntity
        {
            var func = entityCreateExpression.Compile();
            return this.UpdateAndReturnAtPath(path, entity: func(), fields: ODataField.InitializationFields(entityCreateExpression), selectFields: ODataField.Fields(select), upsertBehavior: upsertBehavior);
        }

        #endregion

        #region Delete

        /// <inheritdoc/>
        public IXrmHttpClientAction Delete<T>(Guid id)
            where T : ODataEntity
        {
            return this.DeleteAtPath(ODataPath<T>.FromId(id));
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction Delete<T>(Expression<Func<T, bool>> expression)
            where T : ODataEntity
        {
            return this.DeleteAtPath(ODataPath<T>.FromKeyExpression(expression));
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction DeleteAtPath<T>(ODataPath<T> path)
            where T : ODataEntity
        {
            var requestUri = path.ToUri(this.BaseAddress);

            return new XrmHttpClientQuery<T>(this.Logger, this, HttpMethod.Delete, requestUri);
        }

        #endregion

        #region Associate

        /// <inheritdoc/>
        public IXrmHttpClientAction Associate<T, TOther>(T entity, TOther other, Expression<Func<T, IEnumerable<TOther>>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity
        {
            var entityRecId = ODataEntityContractInfo.GetKeyPropertyValue(entity);
            var otherRecId = ODataEntityContractInfo.GetKeyPropertyValue(other);
            return this.Associate(entityRecId.Value, otherRecId.Value, relationship);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction Associate<T, TOther>(Guid entityRecId, Guid otherRecId, Expression<Func<T, IEnumerable<TOther>>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity
        {
            return this.AssociateAtPath(ODataPath<T>.FromId(entityRecId).Children(relationship), ODataPath<TOther>.FromId(otherRecId));
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction AssociateAtPath<T>(ODataPath<IEnumerable<T>> relationshipPath, ODataPath<T> entityPath)
            where T : ODataEntity
        {
            var requestUri = $"{relationshipPath.ToUri(this.BaseAddress)}/$ref";

            var contentString = new JObject() { { "@odata.id", entityPath.ToFullUri(this.BaseAddress) } }.ToString(Formatting.None);

            return new XrmHttpClientAction(this.Logger, this, HttpMethod.Post, requestUri, contentString);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction Dissociate<T, TOther>(T entity, TOther other, Expression<Func<T, IEnumerable<TOther>>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity
        {
            var entityRecId = ODataEntityContractInfo.GetKeyPropertyValue(entity);
            var otherRecId = ODataEntityContractInfo.GetKeyPropertyValue(other);
            return this.Dissociate(entityRecId.Value, otherRecId.Value, relationship);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction Dissociate<T, TOther>(T entity, Expression<Func<T, TOther>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity
        {
            var entityRecId = ODataEntityContractInfo.GetKeyPropertyValue(entity);
            return this.Dissociate(entityRecId.Value, relationship);
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction Dissociate<T, TOther>(Guid entityRecId, Expression<Func<T, TOther>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity
        {
            return this.DissociateAtPath(ODataPath<T>.FromId(entityRecId).Child(relationship));
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction Dissociate<T, TOther>(Guid entityRecId, Guid otherRecId, Expression<Func<T, IEnumerable<TOther>>> relationship)
            where T : ODataEntity
            where TOther : ODataEntity
        {
            return this.DissociateAtPath(ODataPath<T>.FromId(entityRecId).ChildById(relationship, otherRecId));
        }

        /// <inheritdoc/>
        public IXrmHttpClientAction DissociateAtPath<T>(ODataPath<T> relationshipPath)
            where T : ODataEntity
        {
            var requestUri = $"{relationshipPath.ToUri(this.BaseAddress)}/$ref";

            return new XrmHttpClientAction(this.Logger, this, HttpMethod.Delete, requestUri);
        }
        #endregion

        #region Actions

        /// <inheritdoc/>
        public IXrmHttpClientQuery<TResponse> Action<TRequest, TResponse>(string actionName, TRequest request)
        {
            var requestUri = $"{this.BaseAddress.PathAndQuery}{actionName}";

            var contentString = JsonConvert.SerializeObject(request, CreateJsonSerializerSettings);

            return new XrmHttpClientQuery<TResponse>(this.Logger, this, HttpMethod.Post, requestUri, contentString);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<TResponse> Action<TEntity, TRequest, TResponse>(Guid id, string actionName, TRequest request)
            where TEntity : ODataEntity
        {
            return this.ActionAtPath<TEntity, TRequest, TResponse>(ODataPath<TEntity>.FromId(id), actionName, request);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<TResponse> Action<TEntity, TRequest, TResponse>(Expression<Func<TEntity, bool>> keyExpression, string actionName, TRequest request)
            where TEntity : ODataEntity
        {
            return this.ActionAtPath<TEntity, TRequest, TResponse>(ODataPath<TEntity>.FromKeyExpression(keyExpression), actionName, request);
        }

        /// <inheritdoc/>
        public IXrmHttpClientQuery<TResponse> ActionAtPath<TEntity, TRequest, TResponse>(ODataPath<TEntity> path, string actionName, TRequest request)
            where TEntity : ODataEntity
        {
            var requestUri = $"{path.ToUri(this.BaseAddress)}/{actionName}";

            var contentString = JsonConvert.SerializeObject(request, CreateJsonSerializerSettings);

            return new XrmHttpClientQuery<TResponse>(this.Logger, this, HttpMethod.Post, requestUri, contentString);
        }

        #endregion

        #region Helpers

        /// <inheritdoc/>
        public Task<IOrganizationServiceAsyncDisposable> GetOrganizationService()
        {
            return AuthHeaderOrganizationService.Create(this.BaseAddress.GetLeftPart(UriPartial.Authority), this.GetBearerToken, this.Logger);
        }

        public async Task<HttpResponseMessage> SendAsync(HttpRequestMessage requestMessage, HttpCompletionOption completionOption = HttpCompletionOption.ResponseContentRead)
        {
            var requestId = Guid.NewGuid();

            requestMessage.Headers.Add("User-Agent", $"Dynamics365Talent (RootActivityId/{await this.getRootActivityId()} ActivityVector/{await this.getActivityVector()} XRMRequestId/{requestId})");

            if (ImpersonatedUserObjectId.HasValue)
            {
                requestMessage.Headers.Add("CallerObjectId", ImpersonatedUserObjectId.ToString());
            }
            else if (ImpersonatedUser.HasValue)
            {
                requestMessage.Headers.Add("MSCRMCallerID", ImpersonatedUser.ToString());
            }

            var uri = requestMessage.RequestUri.ToString();
            var uriLength = uri.Length;
            uri = this.ShouldTracePotentialPII
                ? uri
                : XrmHttpClientUriSanitizer.SanitizeUri(requestMessage.RequestUri);

            this.Logger.LogInformation($"XrmHttpClient.SendAsync: Request info: Method={requestMessage.Method} Uri={uri} UriLength={uriLength} Headers=\n{string.Join("\n", requestMessage.Headers.Select(h => h.Key + " : " + string.Join(" ; ", h.Value)))}");

            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await this.GetBearerToken());

            requestMessage.Version = new Version(2, 0);

            var startTime = DateTime.UtcNow;
            HttpResponseMessage response;
            try
            {
                response = await StaticHttpClient.SendAsync(requestMessage, completionOption);
            }
            catch (TaskCanceledException e)
                when (e.InnerException is IOException ioe)
            {
                throw new XrmHttpClientRequestTimeoutException($"{requestMessage.Method} against {this.BaseAddress} timed out after {DateTime.UtcNow - startTime}", e);
            }

            response.Headers.TryGetValues("REQ_ID", out var reqIdHeaders);
            response.Headers.TryGetValues("x-authentication-ticketid", out var authTicketIdHeaders);
            response.Headers.TryGetValues("AuthActivityId", out var authActivityIdHeaders);
            this.Logger.LogInformation($"XrmHttpClient.SendAsync: Executed {requestMessage.Method} against {this.BaseAddress} - REQ_ID={reqIdHeaders?.FirstOrDefault()} AuthActivityId={authTicketIdHeaders?.FirstOrDefault() ?? authActivityIdHeaders?.FirstOrDefault()} - {response.StatusCode} {response.ReasonPhrase} after {DateTime.UtcNow - startTime}");

            return response;
        }


        private async Task<string> GetBearerToken()
        {
            if (this.bearerToken == null)
            {
                this.Logger.LogDebug($"XrmHttpClient.SendAsync: auth header not set, refreshing token");
                await this.RefreshToken();
            }
            else if (this.tokenExpiration != null && DateTime.UtcNow >= this.tokenExpiration)
            {
                this.Logger.LogDebug($"XrmHttpClient.SendAsync: current time {DateTime.UtcNow} after expiration time {this.tokenExpiration}, refreshing token");
                await this.RefreshToken();
            }
            return this.bearerToken;
        }

        private static string AddFilterQueryString(ODataExpression filter, string requestUri)
        {
            var filterString = filter.ToString();
            return string.IsNullOrEmpty(filterString) ? requestUri : QueryHelpers.AddQueryString(requestUri, "$filter", filterString);
        }

        private static string AddFieldListQueryString(string parameter, IEnumerable<ODataField> fields, string requestUri)
        {
            return fields == null ? requestUri : QueryHelpers.AddQueryString(requestUri, parameter, string.Join(",", fields));
        }
        private static string AddLimitQueryString(string parameter, int value, string requestUri)
        {
            return value == 0 ? requestUri : QueryHelpers.AddQueryString(requestUri, parameter, value.ToString());
        }

        private static string AddOrderByQueryString(IEnumerable<Tuple<string, bool>> orderBy, string requestUri)
        {
            return orderBy == null ? requestUri : QueryHelpers.AddQueryString(requestUri, "$orderby", string.Join(",", orderBy.Select(o => o.Item1 + (o.Item2 ? " asc" : " desc"))));
        }

        private static string GetUpdateRequestContent<T>(T entity, IEnumerable<ODataField> fields) where T : ODataEntity
        {
            if (fields == null)
            {
                return JsonConvert.SerializeObject(entity, CreateJsonSerializerSettings);
            }

            var jsonSerializer = JsonSerializer.CreateDefault(DefaultJsonSerializerSettings);
            var entityJObject = JObject.FromObject(entity, jsonSerializer);
            var fieldsToSerialize = fields
                .Select(f => entityJObject.Property(f.ToString())
                          ?? entityJObject.Property(f.ToString() + "@odata.bind"))
                .Where(f => f != null)
                .ToDictionary(f => f.Name, f => f.Value);
            return JsonConvert.SerializeObject(fieldsToSerialize);
        }
        #endregion
    }
}
