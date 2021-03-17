//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Threading.Tasks;

namespace CommonLibrary.Common.XrmHttp
{
    public interface IXrmHttpClientGenerator
    {
        /// <summary>Get an XRM http client on behalf of the calling AAD user.</summary>
        /// <returns>The client.</returns>
        Task<IXrmHttpClient> GetXrmHttpClient();

        /// <summary>Get an XRM http client for the app itself.</summary>
        /// <returns>The client.</returns>
        Task<IXrmHttpClient> GetAdminXrmHttpClient();

        /// <summary>Get an XRM http client to impersonate the specified AAD user.</summary>
        /// <returns>The client.</returns>
        Task<IXrmHttpClient> GetUserImpersonationXrmHttpClient(Guid userObjectId);
    }
}
