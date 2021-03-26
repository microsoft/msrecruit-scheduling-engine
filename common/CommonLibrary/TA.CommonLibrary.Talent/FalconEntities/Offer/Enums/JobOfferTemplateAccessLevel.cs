//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Provisioning.Entities.FalconEntities.Offer
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
