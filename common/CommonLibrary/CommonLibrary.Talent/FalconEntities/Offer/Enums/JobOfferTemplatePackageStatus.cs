//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Provisioning.Entities.FalconEntities.Offer
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
