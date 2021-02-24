//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using Newtonsoft.Json;

namespace MS.GTA.Common.Email.SendGridContracts
{
    /// <summary>
    /// EmailAddress class
    /// </summary>
    public class EmailAddress
    {
        /// <summary>
        /// Gets or sets the email address in email format.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the email address.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}