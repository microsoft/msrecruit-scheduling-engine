// <copyright file="OfferParticipantRole.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer Participant Role
    /// </summary>
    [DataContract]
    public enum OfferParticipantRole
    {
        /// <summary>
        /// Approver
        /// </summary>
        Approver,

        /// <summary>
        /// Fyi
        /// </summary>
        Fyi,

        /// <summary>
        /// Owner
        /// </summary>
        Owner,

        /// <summary>
        /// CoAuthor
        /// </summary>
        CoAuthor
    }
}
