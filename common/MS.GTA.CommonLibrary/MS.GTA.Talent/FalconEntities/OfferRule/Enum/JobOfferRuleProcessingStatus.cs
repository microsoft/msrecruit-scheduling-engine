//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.OfferRule
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferRuleProcessingStatus
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
