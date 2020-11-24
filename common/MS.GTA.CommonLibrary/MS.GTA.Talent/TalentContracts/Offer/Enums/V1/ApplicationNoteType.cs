// <copyright file="ApplicationNoteType.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

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
