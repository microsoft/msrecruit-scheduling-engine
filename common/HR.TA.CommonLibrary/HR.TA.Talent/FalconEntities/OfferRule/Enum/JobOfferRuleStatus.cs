﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Provisioning.Entities.FalconEntities.OfferRule
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