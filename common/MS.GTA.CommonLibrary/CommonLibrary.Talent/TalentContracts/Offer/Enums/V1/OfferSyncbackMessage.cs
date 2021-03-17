//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer Status
    /// </summary>
    [DataContract]
    public enum OfferSyncbackMessage
    {
        /// <summary>
        /// OfferStatusChanged
        /// </summary>
        OfferStatusChanged,
    }
}
