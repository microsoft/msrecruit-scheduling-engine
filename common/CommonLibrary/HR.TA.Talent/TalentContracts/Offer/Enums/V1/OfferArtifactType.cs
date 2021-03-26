//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.Enums.V1
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