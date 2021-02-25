//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Application Note type enum
    /// </summary>
    [DataContract]
    public enum ApplicationNoteType
    {
        /// <summary>
        /// Application Note
        /// </summary>
        Application,

        /// <summary>
        /// Offer Note
        /// </summary>
        Offer
    }
}
