﻿//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>The job offer status for candidate app.</summary>
    [DataContract]
    public enum JobOfferStatus
    {
        /// <summary>Active job offer.</summary>
        Active = 0,

        /// <summary>Inactive job offer.</summary>
        Inactive = 1,

        /// <summary>The pending.</summary>
        Pending = 2,

        /// <summary>The viewed.</summary>
        Viewed = 3,

        /// <summary>The accepted.</summary>
        Accepted = 4,

        /// <summary>The decline.</summary>
        Decline = 5
    }
}