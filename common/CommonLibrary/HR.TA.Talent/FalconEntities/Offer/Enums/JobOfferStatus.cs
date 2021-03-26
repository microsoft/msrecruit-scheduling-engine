//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.Provisioning.Entities.FalconEntities.Offer
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
