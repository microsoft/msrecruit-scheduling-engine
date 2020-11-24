// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferTemplatePackageStatus.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferTemplatePackageStatus
    {
        /// <summary> active </summary>
        Active = 0,

        /// <summary> inactive version </summary>
        Inactive = 1
    }
}