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

    public static class XrmHttpClientExceptionFactory
    {
        public async static Task<XrmHttpClientResponseException> CreateExceptionFromResponse(HttpResponseMessage response)
        {
            var contentString = response.Content == null ? null : await response.Content.ReadAsStringAsync();
            return CreateExceptionFromResponse(response, contentString);
        }

        public static XrmHttpClientResponseException CreateExceptionFromResponse(HttpResponseMessage response, string contentString)
        {
            return XrmHttpClientResponseException.FromResponse(response, contentString);
        }
    }
}
