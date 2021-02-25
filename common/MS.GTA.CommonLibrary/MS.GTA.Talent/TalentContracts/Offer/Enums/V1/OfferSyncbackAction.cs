//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer Status
    /// </summary>
    [DataContract]
    public enum OfferSyncbackAction
    {
        /// <summary>
        /// Create
        /// </summary>
        Create,

        /// <summary>
        /// Update
        /// </summary>
        Update,

        /// <summary>
        /// Delete
        /// </summary>
        Delete
    }
}
