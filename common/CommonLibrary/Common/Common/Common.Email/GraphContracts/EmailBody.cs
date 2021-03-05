//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Email.GraphContracts
{
    using Newtonsoft.Json;

    /// <summary>
    /// Item Body
    /// </summary>
    public class EmailBody
    {
        /// <summary>
        /// Email Body Html
        /// </summary>
        [JsonProperty("content")]
        public string EmailBodyHtml { get; set; }

        /// <summary>
        /// Content Type
        /// </summary>
        [JsonProperty("contentType")]
        public string ContentType { get; set; }
    }
}
