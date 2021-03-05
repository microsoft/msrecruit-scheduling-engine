//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.DocumentDB
{
    using Microsoft.Azure.Documents.Client;
    using CommonDataService.Common.Internal;
    using System.Collections.Generic;

    /// <summary>
    /// Feed Response Extension. Don't delete this extention method. We are using at each service.
    /// </summary>
    public static class FeedResponseExtension
    {
        /// <summary>
        /// Read Feed Response
        /// </summary>
        /// <typeparam name="T">feed response of type T</typeparam>
        /// <param name="feedResponse">feed Response</param>
        /// <returns>List of type T</returns>
        public static IEnumerable<T> ReadFeedResponse<T>(this FeedResponse<T> feedResponse)
        {
            Contract.CheckValue(feedResponse, nameof(feedResponse));

            var results = new List<T>();
            results.AddRange(feedResponse);
            return results;
        }
    }
}
