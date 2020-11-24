// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferArtifactUploadedBy.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferArtifactUploadedBy
    {
        /// <summary> offerManager </summary>
        OfferManager = 0,

        /// <summary> candidate </summary>
        Candidate = 1
    }
}