//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Email properties
    /// </summary>
    [DataContract]
    public class EmailAddress
    {
        /// <summary>
        /// Gets or sets email.
        /// </summary>
        [DataMember(Name = "email", IsRequired = true)]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }
    }
}
