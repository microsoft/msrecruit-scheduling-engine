//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace TA.CommonLibrary.Common.BapClient.Contracts
{
    /// <summary>
    /// The BAP API version
    /// </summary>
    public static class ApiVersion
    {
        /// <summary>
        /// Default API version
        /// </summary>
        public static string DefaultApiVersion => "2016-11-01-alpha";

        /// <summary>
        /// New API version
        /// </summary>
        public static string vNextApiVersion => "2018-01-01-alpha";

        /// <summary>
        /// Checks if it is a valid API version
        /// </summary>
        /// <param name="apiVersion">The BAP API version</param>
        /// <returns>True if it is a valid API version</returns>
        public static bool isValidApiVersion(string apiVersion)
        {
            return apiVersion == DefaultApiVersion
                || apiVersion == vNextApiVersion;
        }
    }
}
