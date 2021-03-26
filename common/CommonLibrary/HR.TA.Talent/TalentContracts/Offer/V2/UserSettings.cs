//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.OfferManagement.Contracts.V2
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The User Settings Contract
    /// </summary>
    [DataContract]
    public class UserSettings
    {
        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets user confirmation settings
        /// </summary>
        [DataMember(Name = "alertSettings", EmitDefaultValue = true)]
        public List<AlertSettings> AlertSettings { get; set; }
    }
}
