//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.MsGraph
{
    /// <summary>
    /// Class to encapsulate all the constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>The graph search query result limit.</summary>
        public const int GraphSearchQueryLimit = 10;

        /// <summary>The bad request error code.</summary>
        public const string BadRequest = "Request_BadRequest";

        /// <summary>The item not found error code.</summary>
        public const string ItemNotFound = "itemNotFound";

        /// <summary>The resource not found error code.</summary>
        public const string ResourceNotFound = "ResourceNotFound";

        /// <summary>The no mail box set up error code.</summary>
        public const string NonExistentMailbox = "ErrorNonExistentMailbox";

        /// <summary>The invalid authentication token code.</summary>
        public const string InvalidAuthenticationToken = "InvalidAuthenticationToken";

        /// <summary>Authorization: identity not found.</summary>
        public const string AuthorizationIdentityNotFound = "Authorization_IdentityNotFound";

        /// <summary>Authentication: unauthorized.</summary>
        public const string AuthenticationUnauthorized = "Authentication_Unauthorized";
    }
}
