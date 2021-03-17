//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Wopi.Interfaces
{
    using Utils;

    /// <summary>
    /// Specifies methods that token helpers must implement
    /// </summary>
    public interface IAccessTokenHelper
    {
        /// <summary>
        /// Validates a JWT
        /// </summary>
        /// <param name="token">JWT Authentication token to validate</param>
        /// <returns>The security token if the token is valid, null otherwise</returns>
        TokenInfo ValidateToken(string token);

        /// <summary>
        /// Generates an access token based off the given input
        /// </summary>
        /// <param name="tokenInfo">The token info object</param>
        /// <returns>The WOPI token string</returns>
        string GenerateToken(TokenInfo tokenInfo);
    }
}
