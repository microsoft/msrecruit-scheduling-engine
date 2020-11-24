// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferRulesetProcessingStatus.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.OfferRule
{
    using System.Runtime.Serialization;

    public enum JobOfferRulesetProcessingStatus
    {
        /// <summary>
        /// Not stared
        /// </summary>
        NotStarted = 0,

        /// <summary>
        /// In Progress
        /// </summary>
        InProgress = 1,

        /// <summary>
        /// Completed
        /// </summary>
        Completed = 2,

        /// <summary>
        /// Error
        /// </summary>
        Error = 3,
    }
}