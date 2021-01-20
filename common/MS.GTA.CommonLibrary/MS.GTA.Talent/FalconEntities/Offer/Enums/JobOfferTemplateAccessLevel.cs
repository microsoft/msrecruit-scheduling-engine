// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferTemplateAccessLevel.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferTemplateAccessLevel
    {
        /// <summary> offerletter </summary>
        Org = 0,

        /// <summary> offerletter </summary>
        User = 1
    }
}