using Newtonsoft.Json;

//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
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