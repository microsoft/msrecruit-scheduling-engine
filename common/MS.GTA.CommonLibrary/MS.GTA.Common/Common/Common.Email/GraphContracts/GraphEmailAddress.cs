using Newtonsoft.Json;

//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Email.GraphContracts
{
    public class GraphEmailAddress
    {
        /// <summary>
        /// Gets or sets the email address in email format.
        /// </summary>
        [JsonProperty("emailAddress")]
        public EmailAddressProperty emailAddress { get; set; }
    }
}
