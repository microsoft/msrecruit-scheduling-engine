//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="Personalization.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Email.SendGridContracts
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Content class
    /// </summary>
    internal class Personalization
    {
        /// <summary>
        /// Gets or sets the emailAddress where the email is sent to
        /// </summary>
        [JsonProperty("to")]
        public IList<EmailAddress> ToEmailAddresses { get; set; }
    }
}
