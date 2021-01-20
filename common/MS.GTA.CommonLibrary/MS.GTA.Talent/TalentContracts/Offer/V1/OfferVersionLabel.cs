// <copyright file="OfferVersionLabel.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Offer Version Title
    /// </summary>
    [DataContract]
    public class OfferVersionLabel
    {
        /// <summary>
        /// Gets or sets Offer ID
        /// </summary>
        [DataMember(Name = "offerID", IsRequired = true, EmitDefaultValue = false)]
        public string OfferID { get; set; }

        /// <summary>
        /// Gets or sets Offer Label
        /// </summary>
        [DataMember(Name = "label", IsRequired = true, EmitDefaultValue = false)]
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the logged user has access
        /// </summary>
        [DataMember(Name = "hasAccess", IsRequired = false, EmitDefaultValue = true)]
        public bool HasAccess { get; set; }
    }
}