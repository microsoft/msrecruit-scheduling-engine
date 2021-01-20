﻿// <copyright file="ApplicationInvitation.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System;

    /// <summary>
    /// Job application Invitation contract.
    /// </summary>
    public class ApplicationInvitation
    {
        /// <summary>Gets or sets the id.</summary>
        public string ID { get; set; }

        /// <summary>Gets or sets the id of Invitation token. </summary>
        public string InvitationTokenId { get; set; }

        /// <summary> Gets or sets the value of Invitation token. </summary>
        public string InvitationTokenValue { get; set; }

        /// <summary> Gets or sets the expiry date time of Invitation token. </summary>
        public DateTime? InvitationTokenExpiryDateTime { get; set; }
    }
}