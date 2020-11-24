// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp
{
    using System.Net.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// An OData query action that can be put in a batch or executed directly.
    /// </summary>
    public interface IXrmHttpClientQuery<T> : IXrmHttpClientAction
    {
        /// <summary>
        /// Execute the query and gets the result.
        /// </summary>
        /// <returns>The result.</returns>
        Task<T> ExecuteAndGetAsync();

        Task<T> HandleQueryResponse(HttpResponseMessage response);
    }
}
