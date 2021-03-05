//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Email.GraphContracts
{
    using Newtonsoft.Json;

    public class EmailAddressProperty
    {
        /// <summary>
        /// Gets or sets the email address in email format.
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the email address.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
