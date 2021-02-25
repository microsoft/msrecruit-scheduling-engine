//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer Template Package Status
    /// </summary>
    [DataContract]
    public enum OfferTemplatePackageStatus
    {
        /// <summary>
        /// Active
        /// </summary>
        Active,

        /// <summary>
        /// Inactive
        /// </summary>
        Inactive
    }
}
