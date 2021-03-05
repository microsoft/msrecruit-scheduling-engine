//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Offer Author Edit Mode
    /// </summary>
    [DataContract]
    public enum OfferAuthorEditMode
    {
        /// <summary> Edit mode </summary>
        Edit,

        /// <summary> ReadOnly mode </summary>
        ReadOnly
    }
}
