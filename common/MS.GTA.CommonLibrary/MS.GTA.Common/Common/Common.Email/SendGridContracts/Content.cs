﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="Content.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Email.SendGridContracts
{
    using Newtonsoft.Json;

    /// <summary>
    /// Content class
    /// </summary>
    internal class Content
    {
        /// <summary>
        /// Gets or sets the email address in email format.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the name associated with the email address.
        /// </summary>
        [JsonProperty("value")]
        public string HtmlBody { get; set; }
    }
}
