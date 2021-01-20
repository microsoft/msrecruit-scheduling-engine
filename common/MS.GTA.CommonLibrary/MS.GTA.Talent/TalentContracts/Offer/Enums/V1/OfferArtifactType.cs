// <copyright file="OfferArtifactType.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for OfferArtifactType
    /// </summary>
    [DataContract]
    public enum OfferArtifactType
    {
        /// <summary>
        /// Offer Letter
        /// </summary>
        OfferLetter,

        /// <summary>
        /// Other Document
        /// </summary>
        OtherDocument,

        /// <summary>
        /// Offer Letter in DocX format
        /// </summary>
        OfferLetterDocX,

        /// <summary>
        /// Offer Tamplate Document
        /// </summary>
        OfferTemplate
    }
}