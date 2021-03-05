//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Wopi.Utils
{
    using System;

    /// <summary>
    /// Constants file
    /// </summary>
    public static class Constants
    {
        /// <summary>User Object ID</summary>
        public const string ObjectId = "oid";

        /// <summary>Tenant Id</summary>
        public const string TenantId = "tid";

        /// <summary>First name</summary>
        public const string FirstName = "given_name";

        /// <summary>User's Surname</summary>
        public const string Surname = "family_name";

        /// <summary>The PUID</summary>
        public const string Puid = "puid";

        /// <summary>The email field</summary>
        public const string Email = "email";

        /// <summary>The environment Id field</summary>
        public const string EnvironmentId = "environmentid";

        /// <summary>The environment mode field</summary>
        public const string EnvironmentMode = "environmentmode";

        /// <summary>The invitation Id field</summary>
        public const string InvitationId = "invitationid";

        /// <summary>The maximum possible expected file size</summary>
        public const int WOPIMaxExpectedSize = int.MaxValue;
    }
}
