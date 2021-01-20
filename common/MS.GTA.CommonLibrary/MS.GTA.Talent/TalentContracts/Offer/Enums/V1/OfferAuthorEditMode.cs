// <copyright file="OfferAuthorEditMode.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
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
