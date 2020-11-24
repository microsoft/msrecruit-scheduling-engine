//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ResponseWithContinuation.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.BapClient.Extensions
{
    using System.Collections;

    /// <summary>
    /// Response with next link signifying continuation.
    /// </summary>
    /// <typeparam name="T">Type of response.</typeparam>
    public class ResponseWithContinuation<T>
        where T : IEnumerable
    {
        /// <summary>
        /// Gets or sets the value of response.
        /// </summary>
        public T Value
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the next link to query to get the remaining results.
        /// </summary>
        public string NextLink
        {
            get;
            set;
        }
    }
}
