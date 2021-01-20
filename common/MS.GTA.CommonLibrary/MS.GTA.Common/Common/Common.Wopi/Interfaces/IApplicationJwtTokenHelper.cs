//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IApplicationJwtTokenHelper.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Wopi.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface IApplicationJwtTokenHelper
    {
        /// <summary>
        /// Validates an access token
        /// </summary>
        /// <param name="token">The token to validate</param>
        /// <returns>A tuple consisting tenantId, environmentId and applicationId if the token is valid</returns>
        Tuple<string, string, string> ReadToken(string token);

        /// <summary>
        /// Gets an access token
        /// </summary>
        /// <param name="tenantId">tenant id</param>
        /// <param name="environmentId">environment id</param>
        /// <param name="applicationId">application id</param>
        /// <returns>The token as a string</returns>
        string GenerateToken(string tenantId, string environmentId, string applicationId);
    }
}
