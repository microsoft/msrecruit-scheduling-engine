//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

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
