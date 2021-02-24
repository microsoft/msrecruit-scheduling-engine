//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.DocumentDB
{
    /// <summary>
    /// Class to encapsulate all the constants.
    /// </summary>
    public class Constants
    {
        /// <summary>Authentication Bearer</summary>
        public const int TooManyRequestCode = 429;

        /// <summary>Default page size for when showing search results.</summary>
        public const int DefaultTake = 20;

        /// <summary>The falcon database id.</summary>
        public static string FalconDatabaseId => "FalconDatabase";

        /// <summary>The auto numbers trigger.</summary>
        public static string AutoNumbersTrigger => "autoNumbersTrigger";

        /// <summary>The runner settings </summary>
        public const string RunnerSettings = "RunnerSettings";

        /// <summary>The local service base url name </summary>
        public const string LocalServiceBaseURI = "http://localhost:19081/";

        /// <summary>The local environment name </summary>
        public const string LocalEnvName = "LOCAL";

        /// <summary>The authority </summary>
        public const string AuthContextUri = "https://login.windows.net/common/";
    }
}
