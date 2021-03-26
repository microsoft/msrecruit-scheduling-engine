//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Offer Artifact Uploaded By
    /// </summary>
    [DataContract]
    public enum OfferArtifactUploadedBy
    {
        /// <summary>
        /// OfferManager
        /// </summary>
        OfferManager,

        /// <summary>
        /// Candidate
        /// </summary>
        Candidate
    }
}
