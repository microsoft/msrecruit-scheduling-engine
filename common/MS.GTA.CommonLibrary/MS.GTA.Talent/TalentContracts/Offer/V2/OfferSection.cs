//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V2
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Specifies the Data Contract for Offer Section
    /// </summary>
    [DataContract]
    public class OfferSection
    {
        /// <summary>
        /// Gets or sets Section Name.
        /// </summary>
        [DataMember(Name = "name", IsRequired = true)]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Section Color.
        /// </summary>
        [DataMember(Name = "color", IsRequired = true)]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets tokens.
        /// </summary>
        [DataMember(Name = "tokens", IsRequired = false)]
        public IList<OfferToken> Tokens { get; set; }
    }
}