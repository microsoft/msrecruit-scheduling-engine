// <copyright file="OfferArtifactUploadedBy.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
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
