//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.BapClient.Contracts
{
    /// <summary>
    /// The BAP expand type
    /// </summary>
    public static class Expand
    {
        /// <summary>
        /// Default no expand call
        /// </summary>
        public static string None => string.Empty;

        /// <summary>
        /// Expand the namespace
        /// </summary>
        public static string Namespace => "&$expand=namespace";

        /// <summary>
        /// Expand the permissions
        /// </summary>
        public static string Permissions => "&$expand=permissions";

        /// <summary>
        /// Validates that a string is a valid BAP expand query
        /// </summary>
        /// <param name="expandString">The string to check</param>
        /// <returns>True if it is a valid string</returns>
        public static bool isValidExpandString(string expandString)
        {
            return expandString == None
                || expandString == Namespace
                || expandString == Permissions;
        }
    }
}
