// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferRulesetStatus.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.XrmEntities.OfferRule
{
    using System.Runtime.Serialization;

    public enum JobOfferRulesetStatus
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
