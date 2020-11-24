//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Microsoft.AspNetCore.Http.Extensions;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Primitives;

    public static class XrmHttpClientActionExtensions
    {
        public static IXrmHttpClientQuery<ODataResponseList<T>> OrderByAscending<T>(this IXrmHttpClientQuery<ODataResponseList<T>> query, params ODataField[] fields)
            where T : ODataEntity
        {
            query.RequestUri = AddToQueryParameterList(query.RequestUri, "$orderby", fields?.Select(f => f + " asc"));
            return query;
        }

        public static IXrmHttpClientQuery<ODataResponseList<T>> OrderByAscending<T>(this IXrmHttpClientQuery<ODataResponseList<T>> query, Expression<Func<T, object>> fieldExpression)
            where T : ODataEntity
        {
            return query.OrderByAscending(ODataField.Fields(fieldExpression)?.ToArray());
        }

        public static IXrmHttpClientQuery<ODataResponseList<T>> OrderByDescending<T>(this IXrmHttpClientQuery<ODataResponseList<T>> query, params ODataField[] fields)
            where T : ODataEntity
        {
            query.RequestUri = AddToQueryParameterList(query.RequestUri, "$orderby", fields?.Select(f => f + " desc"));
            return query;
        }

        public static IXrmHttpClientQuery<ODataResponseList<T>> OrderByDescending<T>(this IXrmHttpClientQuery<ODataResponseList<T>> query, Expression<Func<T, object>> fieldExpression)
            where T : ODataEntity
        {
            return query.OrderByDescending(ODataField.Fields(fieldExpression)?.ToArray());
        }

        /// <summary>
        /// Add select clause to a query.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="query">The query object.</param>
        /// <param name="fields">The fields to expand.</param>
        /// <returns>The query object.</returns>
        public static IXrmHttpClientQuery<T> Select<T>(this IXrmHttpClientQuery<T> query, params ODataField[] fields)
        {
            query.RequestUri =
                AddToQueryParameterList(
                    query.RequestUri,
                    "$select",
                    fields.Select(f => f.ToString()));
            return query;
        }

        /// <summary>
        /// Add a select clause to a query.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="query">The query object.</param>
        /// <param name="fieldExpression">The fields to select.</param>
        /// <returns>The query object.</returns>
        public static IXrmHttpClientQuery<ODataResponseList<T>> Select<T>(
                this IXrmHttpClientQuery<ODataResponseList<T>> query,
                Expression<Func<T, object>> fieldExpression)
            where T : ODataEntity
        {
            return query.Select(ODataField.Fields(fieldExpression).ToArray());
        }

        /// <summary>
        /// Add a select clause to a query.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="query">The query object.</param>
        /// <param name="fieldExpression">The fields to select.</param>
        /// <returns>The query object.</returns>
        public static IXrmHttpClientQuery<T> Select<T>(
                this IXrmHttpClientQuery<T> query,
                Expression<Func<T, object>> fieldExpression)
            where T : ODataEntity
        {
            return query.Select(ODataField.Fields(fieldExpression).ToArray());
        }

        /// <summary>
        /// Add an expand clause to a query.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="query">The query object.</param>
        /// <param name="field">The field to expand.</param>
        /// <param name="select">The list of child fields to select; optional.</param>
        /// <param name="filterExpression">The instnce of <see cref="ODataExpression"/>.</param>
        /// <param name="top">The number of items for query.</param>
        /// <returns>The query object.</returns>
        public static IXrmHttpClientQuery<T> ExpandODataField<T>(
            this IXrmHttpClientQuery<T> query,
            ODataField field,
            IEnumerable<ODataField> select = null,
            ODataExpression filterExpression = default(ODataExpression),
            int top = 0)
        {
            var parameters = new List<string>();
            if (select != null)
            {
                parameters.Add($"$select={string.Join(",", select.Select(s => s.ToString()))}");
            }

            if (filterExpression != default(ODataExpression))
            {
                parameters.Add($"$filter={filterExpression}");
            }

            if (top > 0)
            {
                parameters.Add($"$top={top}");
            }

            query.RequestUri =
                AddToQueryParameterList(
                    query.RequestUri,
                    "$expand",
                    new[] { field + (parameters.Any() ? $"({string.Join(";", parameters)})" : string.Empty) });
            return query;
        }

        /// <summary>
        /// Add an expand clause to a query.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TChild">The child entity type.</typeparam>
        /// <param name="query">The query object.</param>
        /// <param name="field">The field to expand.</param>
        /// <param name="select">The list of child fields to select; optional.</param>
        /// <returns>The query object.</returns>
        public static IXrmHttpClientQuery<ODataResponseList<T>> Expand<T, TChild>(
                this IXrmHttpClientQuery<ODataResponseList<T>> query,
                Expression<Func<T, TChild>> field,
                Expression<Func<TChild, object>> select = null)
            where T : ODataEntity
            where TChild : ODataEntity
        {
            return query.ExpandODataField(ODataField.Field(field), ODataField.Fields(select));
        }

        /// <summary>
        /// Add an expand clause to a query.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TChild">The child entity type.</typeparam>
        /// <param name="query">The query object.</param>
        /// <param name="field">The field to expand.</param>
        /// <param name="select">The list of child fields to select; optional.</param>
        /// <returns>The query object.</returns>
        public static IXrmHttpClientQuery<T> Expand<T, TChild>(
                this IXrmHttpClientQuery<T> query,
                Expression<Func<T, TChild>> field,
                Expression<Func<TChild, object>> select = null)
            where T : ODataEntity
            where TChild : ODataEntity
        {
            return query.ExpandODataField(ODataField.Field(field), ODataField.Fields(select));
        }

        /// <summary>
        /// Add an expand clause to a query.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TChild">The child entity type.</typeparam>
        /// <param name="query">The query object.</param>
        /// <param name="field">The field to expand.</param>
        /// <param name="select">The list of child fields to select; optional.</param>
        /// <param name="filter">The filter expression; optional.</param>
        /// <param name="top">The number of items in query.</param>
        /// <returns>The query object.</returns>
        [Obsolete("Use ExpandAll to avoid needing type parameters to avoid ambiguity", false)]
        public static IXrmHttpClientQuery<T> Expand<T, TChild>(
                this IXrmHttpClientQuery<T> query,
                Expression<Func<T, IEnumerable<TChild>>> field,
                Expression<Func<TChild, object>> select = null,
                Expression<Func<TChild, bool>> filter = null,
                int top = 0)
            where T : ODataEntity
            where TChild : ODataEntity
        {
            return query.ExpandODataField(
                ODataField.Field(field),
                ODataField.Fields(select),
                ODataExpression.Filter(filter),
                top);
        }

        /// <summary>
        /// Add an expand clause to a query.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <typeparam name="TChild">The child entity type.</typeparam>
        /// <param name="query">The query object.</param>
        /// <param name="field">The field to expand.</param>
        /// <param name="select">The list of child fields to select; optional.</param>
        /// <param name="filter">The filter expression; optional.</param>
        /// <param name="top">The number of top items to be queried.</param>
        /// <returns>The query object.</returns>
        public static IXrmHttpClientQuery<T> ExpandAll<T, TChild>(
                this IXrmHttpClientQuery<T> query,
                Expression<Func<T, IEnumerable<TChild>>> field,
                Expression<Func<TChild, object>> select = null,
                Expression<Func<TChild, bool>> filter = null,
                int top = 0)
            where T : ODataEntity
            where TChild : ODataEntity
        {
            return query.ExpandODataField(
                ODataField.Field(field),
                ODataField.Fields(select),
                ODataExpression.Filter(filter),
                top);
        }

        private static string AddToQueryParameterList(string uri, string paramName, IEnumerable<string> addedValues)
        {
            if (addedValues == null)
            {
                return uri;
            }

            var urlParts = uri.ToString().Split(new[] { '?' }, 2);

            var queryValues = urlParts.Length == 1 ? new Dictionary<string, StringValues>() : QueryHelpers.ParseQuery(urlParts[1]);
            if (queryValues.TryGetValue(paramName, out var existingValues))
            {
                queryValues.Remove(paramName);
            }

            var queryBuilder = new QueryBuilder(queryValues.ToDictionary(q => q.Key, q => string.Join(",", q.Value)))
            {
                { paramName, string.Join(",", existingValues.Concat(addedValues)) }
            };

            return urlParts[0] + queryBuilder.ToQueryString();
        }
    }
}
