//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.BusinessLibrary.Requests
{
    using System.IO;

    /// <summary>
    /// Extend Offer Request
    /// </summary>
    public class DeleteOfferArtifactRequest : BaseRequest
    {
        /// <summary>
        /// Gets or sets Offer Id
        /// </summary>
        public string OfferId { get; set; }

        /// <summary>
        /// Gets or sets Offer Artifact Id
        /// </summary>
        public string OfferArtifactId { get; set; }
    }
}
