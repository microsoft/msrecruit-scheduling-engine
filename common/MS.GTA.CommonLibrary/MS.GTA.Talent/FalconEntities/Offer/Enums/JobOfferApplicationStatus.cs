// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferApplicationStatus.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferApplicationStatus
    {
        /// <summary> New application whose offer is not yet prepared </summary>
        New = 0,

        /// <summary> Offer preparation has been initiated </summary>
        OfferPrepared = 1,
    }
}