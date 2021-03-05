//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for offer artifact document type
    /// </summary>
    [DataContract]
    public enum OfferArtifactDocumentType
    {
        /// <summary>
        /// Doc
        /// </summary>
        Doc,

        /// <summary>
        /// DocX
        /// </summary>
        DocX,

        /// <summary>
        /// Pdf
        /// </summary>
        Pdf,

        /// <summary>
        /// Html
        /// </summary>
        Html,

        /// <summary>
        /// Jpg
        /// </summary>
        Jpg,

        /// <summary>
        /// XlsX
        /// </summary>
        XlsX,

        /// <summary>
        /// PptX
        /// </summary>
        PptX,

        /// <summary>
        /// Jpeg
        /// </summary>
        Jpeg,

        /// <summary>
        /// Png
        /// </summary>
        Png,

        /// <summary>
        /// Txt
        /// </summary>
        Txt
    }
}
