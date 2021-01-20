// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An OData action that can be put in a batch or executed directly.
    /// </summary>
    public interface IXrmHttpClientAction
    {
        HttpMethod Method { get; set; }

        string RequestUri { get; set; }

        string Content { get; set; }

        IEnumerable<Tuple<string, string>> Headers { get; }

        IXrmHttpClient Client { get; }

        /// <summary>
        /// The index of the action in the batch.
        /// </summary>
        /// <remarks>Reference in subsequent actions with ODataExpression.ContentId or ODataEntity.ODataBatchContentIdReference.</remarks>
        int ContentId { get; set; }

        /// <summary>
        /// Execute the action.
        /// </summary>
        /// <returns>The task to await.</returns>
        Task ExecuteAsync();

        Task<Guid?> HandleActionResponse(HttpResponseMessage response);

        HttpContent ToHttpContent();
    }
}