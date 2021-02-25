//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferTemplatePackageStatusReason
    {
        /// <summary> draft </summary>
        Draft = 0,

        /// <summary> published </summary>
        Published = 1,

        /// <summary> new version created</summary>
        Versioned  = 2
    }
}
