// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferRuleStatus.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.OfferRule
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferRuleStatus
    {
        /// <summary>
        /// Active
        /// </summary>
        Active = 0,

        /// <summary>
        /// Inactive
        /// </summary>
        Inactive = 1
    }
}