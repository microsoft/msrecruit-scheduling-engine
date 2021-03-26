//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferArtifactType
    {
        /// <summary> offerletter </summary>
        OfferLetter = 0,

        /// <summary> otherdocument </summary>
        OtherDocument = 1,

        /// <summary> offerletterdocx </summary>
        OfferLetterDocX = 2, 

        /// <summary>
        /// Offer Tamplate Document
        /// </summary>
        OfferTemplate = 3
    }
}