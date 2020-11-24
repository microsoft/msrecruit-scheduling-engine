// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferStatus.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferStatus
    {
        /// <summary> active </summary>
        Active = 0,

        /// <summary> inactive </summary>
        Inactive =  1
    }
}
