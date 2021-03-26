//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.XrmHttp
{
    using System;
    using System.Text.RegularExpressions;

    public class ODataUriParse
    {
        /// <summary>
        /// Matches the end of a uri like `/base/JobOpenings(8a3ed1f5-0f03-41de-a617-b033c3361f60)`
        /// </summary>
        private static readonly Regex IdUrlParseRegex = new Regex(@"/([a-zA-Z0-9_-]+)\(([a-fA-F0-9-]+)\)$");

        /// <summary>
        /// Get the entity guid from a uri like `/base/JobOpenings(8a3ed1f5-0f03-41de-a617-b033c3361f60)`
        /// </summary>
        /// <param name="uri">The uri to parse.</param>
        /// <returns>The entity id.</returns>
        public static Guid GetEntityIdFromUri(string uri)
        {
            return TryGetEntityIdFromUri(uri) 
                ?? throw new InvalidOperationException($"Could not get entity id from uri {uri}");
        }

        /// <summary>
        /// Get the entity guid from a uri like `/base/JobOpenings(8a3ed1f5-0f03-41de-a617-b033c3361f60)`
        /// </summary>
        /// <param name="uri">The uri to parse.</param>
        /// <returns>The entity id.</returns>
        public static Guid? TryGetEntityIdFromUri(string uri)
        {
            if (string.IsNullOrEmpty(uri))
            {
                return null;
            }

            var match = IdUrlParseRegex.Match(uri);
            if (match == null || !match.Success)
            {
                return null;
            }

            if (Guid.TryParse(match.Groups[2].Value, out var guid))
            {
                return guid;
            }

            return null;
        }
    }
}
