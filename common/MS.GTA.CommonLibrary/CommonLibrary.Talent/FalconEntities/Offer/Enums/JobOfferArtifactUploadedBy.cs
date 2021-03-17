//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Provisioning.Entities.FalconEntities.Offer
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
