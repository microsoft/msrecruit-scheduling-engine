﻿// <copyright file="ApplicationActivityParticipant.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System;
    using MS.GTA.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// The participant for a job application activity.
    /// </summary>
    public class ApplicationActivityParticipant
    {
        /// <summary>
        /// Gets or sets the participant identifier for the job application activity.
        /// </summary>
        public string ApplicationActivityParticipantId { get; set; }

        /// <summary>
        /// Gets or sets the graph object identifier of the job opening participant.
        /// </summary>
        public string JobOpeningParticipantObjectId { get; set; }

        /// <summary>
        /// Gets or sets the role of the job opening participant.
        /// </summary>
        public OpeningParticipantRole? JobOpeningParticipantRole { get; set; }

        /// <summary>
        /// Gets or sets identifier of the job application participant.
        /// </summary>
        public string JobApplicationParticipantId { get; set; }
    }
}
