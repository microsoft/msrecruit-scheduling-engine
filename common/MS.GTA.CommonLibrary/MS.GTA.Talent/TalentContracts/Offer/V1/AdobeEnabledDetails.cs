//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for Adobe Sign Enable
    /// </summary>
    [DataContract]
    public class AdobeEnabledDetails
    {
        /// <summary>
        /// Gets or sets a value indicating whether is Adobe sign enabled
        /// </summary>
        [DataMember(Name = "isEnabled", IsRequired = true)]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Gets or sets client ID
        /// </summary>
        [DataMember(Name = "clientId", IsRequired = false)]
        public string ClientId { get; set; }
    }
}
