//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="OfferNoteContent.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V2
{
    using System.IO;

    /// <summary>
    /// The Offer Artifact data contract
    /// </summary>
    public class OfferNoteContent
    {
        /// <summary>
        /// Gets or sets name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Contents.
        /// </summary>
        public Stream Content { get; set; }
    }
}