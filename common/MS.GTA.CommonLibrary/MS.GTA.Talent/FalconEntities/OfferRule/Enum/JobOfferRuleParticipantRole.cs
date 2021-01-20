// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferRuleParticipantRole.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.OfferRule
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferRuleParticipantRole
    {
        /// <summary>
        /// Not stared
        /// </summary>
        Owner = 0,

        /// <summary>
        /// Contributor
        /// </summary>
        Contributor = 1,
    }
}
