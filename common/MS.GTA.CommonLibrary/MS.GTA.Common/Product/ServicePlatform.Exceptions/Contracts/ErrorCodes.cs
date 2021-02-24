//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.ServicePlatform.Exceptions
{
    public static class ErrorCodes
    {
        /// <summary>
        /// Use this error for generic unexpected service errors where a more specific error 
        /// code does not need to be surfaced to the clients.
        /// </summary>
        public const string GenericServiceError = "UnknownError";

        /// <summary>
        /// Use this error for generic authentication errors.
        /// </summary>
        public const string GenericAuthenticationError = "AuthenticationFailed";

        /// <summary>
        /// Use this error for bad request (400) errors.
        /// </summary>
        public const string BadRequestStatusError = "BadRequest";

        /// <summary>
        /// Use this error for unauthorized (401) errors.
        /// </summary>
        public const string UnauthorizedStatusError = "Unauthorized";

        /// <summary>
        /// Use this error for forbidden (403) errors.
        /// </summary>
        public const string ForbiddenStatusError = "Forbidden";

        /// <summary>
        /// Use this error for not found (404) errors.
        /// </summary>
        public const string NotFoundStatusError = "NotFound";

        /// <summary>
        /// Use this error for conflict (409) errors.
        /// </summary>
        public const string ConflictStatusError = "Conflict";
    }
}
