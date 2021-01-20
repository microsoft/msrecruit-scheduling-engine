//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EmailAddress.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V1
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
