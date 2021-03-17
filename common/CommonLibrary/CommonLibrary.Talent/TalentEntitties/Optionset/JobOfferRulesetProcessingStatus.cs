//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//------------------------------------------------------------------------
namespace CommonLibrary.Common.Provisioning.Entities.XrmEntities.OfferRule
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
