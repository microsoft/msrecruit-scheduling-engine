// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferTemplateStatus.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferTemplateStatus
    {
        /// <summary> active </summary>
        Active = 0,

        /// <summary> archive </summary>
        Archive = 1,

        /// <summary> draft version </summary>
        Draft =2,

        /// <summary> inactive version </summary>
        Inactive = 3
    }
}