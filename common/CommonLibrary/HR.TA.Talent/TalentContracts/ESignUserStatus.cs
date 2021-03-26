//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using HR.TA..TalentEngagementService.Data;

    /// <summary>
    /// Specifies the Data Contract for ESign Account (This will be moved to Key Vault)
    /// </summary>
    [DataContract]
    public class ESignUserStatus
    {
        /// <summary>
        /// Gets or sets a value indicating whether IsEsignEnabled enabled or not
        /// </summary>
        [DataMember(Name = "isEsignEnabled", IsRequired = false)]
        public bool IsEsignEnabled { get; set; }

        /// <summary>
        /// Gets or sets IntegrationKey
        /// </summary>
        [DataMember(Name = "integrationKey", IsRequired = false)]
        public string IntegrationKey { get; set; }

        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        [DataMember(Name = "emailAddress", IsRequired = false)]
        public string EmailAddress { get; set; }
    }
}
