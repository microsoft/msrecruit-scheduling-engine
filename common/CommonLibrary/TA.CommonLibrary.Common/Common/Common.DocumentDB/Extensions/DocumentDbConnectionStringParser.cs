//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.DocumentDB.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// A static class for handling document DB connection strings
    /// </summary>
    public static class DocumentDbConnectionStringParser
    {
        /// <summary>The document DB uri identifier.</summary>
        private const string Account = "AccountEndpoint";

        /// <summary>The document DB key.</summary>
        private const string AccountKey = "AccountKey";

        /// <summary>The document DB connection string.</summary>
        private static string connectionString;

        /// <summary>The document DB parameters dictionary.</summary>
        private static Dictionary<string, string> values;

        /// <summary>
        /// Gets the document DB Access Key for the given connection string.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <returns>The document DB Account Key.</returns>
        public static string GetResourceAccessKey(string connectionString)
        {
            return GetValue(connectionString, AccountKey);
        }

        /// <summary>
        /// Gets the document DB database Resource Endpoint A.K.A the Account Endpoint.
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <returns>The document DB resource endpoint.</returns>
        public static string GetResourceEndpoint(string connectionString)
        {
            return GetValue(connectionString, Account);
        }

        /// <summary>
        /// Gets an arbitrary value from the document DB key value pair store
        /// </summary>
        /// <param name="connectionString">The connection string to use</param>
        /// <param name="dictionaryKey">The key to look up</param>
        /// <returns>The retrieved value</returns>
        private static string GetValue(string connectionString, string dictionaryKey)
        {
            EnsureConnectionStringIsValid(connectionString);

            string dictionaryValue;
            if (values.TryGetValue(dictionaryKey, out dictionaryValue))
            {
                return dictionaryValue;
            }

            return string.Empty;
        }

        /// <summary>
        /// Ensures that the document DB connection string is valid and updates the dictionary if it is outdated
        /// </summary>
        /// <param name="inputConnectionString">The new connection string to use</param>
        private static void EnsureConnectionStringIsValid(string inputConnectionString)
        {
            if (!string.Equals(connectionString, inputConnectionString, StringComparison.OrdinalIgnoreCase))
            {
                values = inputConnectionString.Split(';')
                                                    .Where(kvp => kvp.Contains('='))
                                                    .Select(kvp => kvp.Split(new char[] { '=' }, 2))
                                                    .ToDictionary(
                                                        kvp => kvp[0].Trim(),
                                                        kvp => kvp[1].Trim(),
                                                        StringComparer.InvariantCultureIgnoreCase);
                connectionString = inputConnectionString;
            }
        }
    }
}
